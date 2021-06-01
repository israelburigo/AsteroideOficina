using AsteroideOficina.Engine;
using AsteroideOficina.Entidades;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroideOficina.Geradores
{
    public class GeradorMeteoros : GameComponent
    {
        public float Tempo { get; set; } = 2f;

        public GeradorMeteoros(Game game)
            : base(game)
        {
            game.Components.Add(this);
        }

        public override void Update(GameTime gameTime)
        {
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if ((Tempo -= dt) > 0)
                return;

            Tempo = 2f;

            Gerar(1);
        }

        public void Gerar(int quant)
        {
            for (int i = 0; i < quant; i++)
                Gerar();
        }

        private void Gerar()
        {
            var rot = new MinMax(0.001f, 0.05f).Random();
            var vel = new MinMax(10f, 100f).Random();

            var bordaInfSup = new MinMax(0, Game.Window.ClientBounds.Width).RandomInt();
            var bordaEsqDir = new MinMax(0, Game.Window.ClientBounds.Height).RandomInt();

            var cantos = new MinMax(0, 3).RandomInt();

            Vector2 pos;
            switch (cantos)
            {
                case 0: pos = new Vector2(bordaInfSup, 0); break;
                case 1: pos = new Vector2(bordaInfSup, Game.Window.ClientBounds.Height); break;
                case 2: pos = new Vector2(0, bordaEsqDir); break;
                case 3: pos = new Vector2(Game.Window.ClientBounds.Width, bordaEsqDir); break;
                default: pos = Vector2.Zero; break;
            }

            var values = Enum.GetValues(typeof(EnumTipo));
            var index = new MinMax(0, values.Length - 1).RandomInt();
            new Meteoro(Game, (EnumTipo)values.GetValue(index))
            {
                Posicao = new Vector2(pos.X, pos.Y),
                Rotacao = rot,
                Velocidade = vel,
                Direcao = GeradorDirecao(pos, Globals.Player.Posicao)
            };
        }

        private Vector2 GeradorDirecao(Vector2 pos, Vector2 plPos)
        {
            var dx = plPos.X - pos.X;
            var dy = plPos.Y - pos.Y;
            var dir = new Vector2(dx, dy);
            dir.Normalize();
            return dir;
        }
    }
}
