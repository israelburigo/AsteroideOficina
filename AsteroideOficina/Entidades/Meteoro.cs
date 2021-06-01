using AsteroideOficina.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroideOficina.Entidades
{
    public enum EnumTipo
    {
        Meteorao,
        Meteoro,
        Meteorinho
    }

    public class Meteoro : DrawableGameComponent
    {
        public Texture2D Textura { get; set; }
        public Vector2 Posicao { get; set; }
        public Vector2 Direcao { get; set; }
        public float Rotacao { get; set; }
        public float Velocidade { get; set; }

        public float Raio { get { return Textura.Width / 2; } }

        public Meteoro(Game game, EnumTipo tipo) 
            : base(game)
        {
            game.Components.Add(this);

            if (tipo == EnumTipo.Meteorao)
                Textura = game.Content.Load<Texture2D>("2d/meteorao");

            if (tipo == EnumTipo.Meteoro)
                Textura = game.Content.Load<Texture2D>("2d/meteoro");

            if (tipo == EnumTipo.Meteorinho)
                Textura = game.Content.Load<Texture2D>("2d/meteorinho");
        }

        public override void Update(GameTime gameTime)
        {
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Posicao += Direcao * dt * Velocidade;

            Rotacao += 0.01f;

            if (Posicao.X < 0)
                Posicao = new Vector2(Game.Window.ClientBounds.Width - 1, Posicao.Y);

            if (Posicao.X > Game.Window.ClientBounds.Width)
                Posicao = new Vector2(0, Posicao.Y);

            if (Posicao.Y < 0)
                Posicao = new Vector2(Posicao.X, Game.Window.ClientBounds.Height - 1);

            if (Posicao.Y > Game.Window.ClientBounds.Height)
                Posicao = new Vector2(Posicao.X, 0);
        }

        public override void Draw(GameTime gameTime)
        {
            Globals.SpriteBatch.Draw(Textura, Posicao, null, Color.White, Rotacao, new Vector2(Textura.Width/2, Textura.Height/2), 1f, SpriteEffects.None, 0);
        }
    }
}
