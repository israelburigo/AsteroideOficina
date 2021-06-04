using AsteroideOficina.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AsteroideOficina.Engine.Extension;
using AsteroideOficina.Entidades;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace AsteroideOficina
{
    public class Player : DrawableGameComponent, IPlayer
    {
        public Texture2D Textura { get; set; }
        public Vector2 Posicao { get; set; }
        public Vector2 Inercia { get; set; }
        public Vector2 Direcao { get; set; }
        public List<Vector2> Colisao { get; set; } = new List<Vector2>();
        private float _tempoTiro = 0f;
        public SoundEffect SomDoTirinho { get; set; }
        public SoundEffect SomMorreu { get; set; }

        public int Pontos { get; set; }

        public Player(Game game) : base(game)
        {
            game.Components.Add(this);

            Textura = game.Content.Load<Texture2D>("2d/player");
            Posicao = new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            Direcao = new Vector2(1f, 0f);
            SomDoTirinho = game.Content.Load<SoundEffect>("sounds/tiro");
            SomMorreu = game.Content.Load<SoundEffect>("sounds/explosaoNave");
        }

        internal void Iniciar()
        {
            Posicao = new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            Direcao = new Vector2(1f, 0f);
            Inercia = Vector2.Zero;
            Pontos = 0;
        }

        public override void Update(GameTime gameTime)
        {
            var game = (Game as Main);
            if (!game.Comecou)
                return;

            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds; //16ms

            var keys = Keyboard.GetState().GetPressedKeys().ToList();

            if (keys.Contains(Keys.Up))
                Inercia -= Direcao * 10;

            if (keys.Contains(Keys.Down))
                Inercia += Direcao * 5;

            if (keys.Contains(Keys.Right))
                Direcao = Direcao.Rotate(-0.1f);

            if (keys.Contains(Keys.Left))
                Direcao = Direcao.Rotate(0.1f);

            if (Posicao.X < 0)
                Posicao = new Vector2(Game.Window.ClientBounds.Width - 1, Posicao.Y);

            if (Posicao.X > Game.Window.ClientBounds.Width)
                Posicao = new Vector2(0, Posicao.Y);

            if (Posicao.Y < 0)
                Posicao = new Vector2(Posicao.X, Game.Window.ClientBounds.Height - 1);

            if (Posicao.Y > Game.Window.ClientBounds.Height)
                Posicao = new Vector2(Posicao.X, 0);

            Posicao += Inercia * dt;

            var meteoros = Game.Components.OfType<Meteoro>().ToList();

            Colisao = new List<Vector2>
            {
                new Vector2(Posicao.X, Posicao.Y - Textura.Height/2).Rotate(Direcao.Angulo(), Posicao),
                new Vector2(Posicao.X - Textura.Width/2, Posicao.Y + Textura.Height/2).Rotate(Direcao.Angulo(), Posicao),
                new Vector2(Posicao.X + Textura.Width/2, Posicao.Y + Textura.Height/2).Rotate(Direcao.Angulo(), Posicao),
                Posicao
            };

            if (keys.Contains(Keys.Space))
                Atira(dt);

            if (!keys.Contains(Keys.Space))
                _tempoTiro = 0;

            foreach (var m in meteoros)
            {
                foreach (var col in Colisao)
                {
                    var dist = Vector2.Distance(m.Posicao, col);
                    if (dist < m.Raio)
                    {
                        SomMorreu.Play();
                        game.Comecou = false;

                        new Particulas
                        {
                            Angulo = new MinMax(0, 359),
                            Posicao = Posicao,
                            TempoDeVida = new MinMax(1f, 3f),
                            Velocidade = new MinMax(10f, 100f),
                            Textura = Game.Content.Load<Texture2D>("2d/particula"),
                            Cor = Color.Red
                        }.Start(Game, 100);
                    }
                }
            }

            base.Update(gameTime);
        }

      
        private void Atira(float dt)
        {
            if ((_tempoTiro -= dt) > 0)
                return;

            _tempoTiro = 0.5f;
            

            var ponto = Colisao.First();

            new Tiro(Game)
            {
                Posicao = new Vector2(ponto.X, ponto.Y),
                Direcao = new Vector2(Direcao.X, Direcao.Y),
                Velocidade = 500f
            };

            SomDoTirinho.Play();            
        }

        public override void Draw(GameTime gameTime)
        {
            var game = (Game as Main);
            if (!game.Comecou)
                return;

            Globals.SpriteBatch.Draw(Textura, Posicao, null, Color.White, -Direcao.Angulo(), new Vector2(Textura.Width / 2, Textura.Width / 2), 1f, SpriteEffects.None, 0);

            //Colisao.ForEach(p =>
            //{
            //    Globals.SpriteBatch.DrawPoint(p, 3, Color.Red);
            //});
        }
    }
}
