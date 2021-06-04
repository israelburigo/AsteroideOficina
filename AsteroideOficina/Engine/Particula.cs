using AsteroideOficina.Engine.Extension;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroideOficina.Engine
{
    public class Particula : DrawableGameComponent
    {
        public Particulas Parent { get; set; }
        public Vector2 Posicao { get; set; }
        public float Velocidade { get; set; }
        public Vector2 Direcao { get; set; }
        public float TempoDeVida { get; set; }

        public Particula(Game game, Particulas particulas) 
            : base(game)
        {
            game.Components.Add(this);
            Parent = particulas;

            Posicao = new Vector2(Parent.Posicao.X, Parent.Posicao.Y);
            Velocidade = Parent.Velocidade.Random();
            Direcao = Vector2.UnitX.Rotate(MathHelper.ToRadians(Parent.Angulo.Random()));
            TempoDeVida = Parent.TempoDeVida.Random();
        }

        public override void Update(GameTime gameTime)
        {
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Posicao += Direcao * Velocidade * dt;

            TempoDeVida -= dt;
            if (TempoDeVida < 0)
                Game.Components.Remove(this);
        }

        public override void Draw(GameTime gameTime)
        {
            Globals.SpriteBatch.Draw(Parent.Textura, Posicao, Parent.Cor);
        }
    }
}
