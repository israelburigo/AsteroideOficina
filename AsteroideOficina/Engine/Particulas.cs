using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroideOficina.Engine
{
    public class Particulas
    {
        public MinMax Angulo { get; set; }
        public MinMax Velocidade { get; set; }
        public MinMax TempoDeVida { get; set; }
        public Texture2D Textura { get; set; }
        public Vector2 Posicao { get; set; }
        public Color Cor { get; set; }

        public void Start(Game game, int quant)
        {
            for (int i = 0; i < quant; i++)
                new Particula(game, this);
        }

    }
}
