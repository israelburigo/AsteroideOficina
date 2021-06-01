﻿using AsteroideOficina.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AsteroideOficina.Engine.Extension;
using AsteroideOficina.Entidades;

namespace AsteroideOficina
{
    public class Player : DrawableGameComponent
    {
        public Texture2D Textura { get; set; }
        public Vector2 Posicao { get; set; }
        public Vector2 Inercia { get; set; }
        public Vector2 Direcao { get; set; }
        public List<Vector2> Colisao { get; set; } = new List<Vector2>();

        public Player(Game game) : base(game)
        {
            game.Components.Add(this);

            Textura = game.Content.Load<Texture2D>("2d/player");
            Posicao = new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            Direcao = new Vector2(1f, 0f);
        }

        public override void Update(GameTime gameTime)
        {
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds; //16ms

            var keys = Keyboard.GetState().GetPressedKeys().ToList();

            if (keys.Contains(Keys.Up))
                Inercia += Direcao * 10;

            if (keys.Contains(Keys.Down))
                Inercia -= Direcao * 5;

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

            var meteoros = Game.Components.OfType<Meteoro>();

            Colisao = new List<Vector2>
            {
                new Vector2(Posicao.X, Posicao.Y + Textura.Height/2).Rotate(Direcao.Angulo(), Posicao),
                new Vector2(Posicao.X - Textura.Width/2, Posicao.Y - Textura.Height/2).Rotate(Direcao.Angulo(), Posicao),
                new Vector2(Posicao.X + Textura.Width/2, Posicao.Y - Textura.Height/2).Rotate(Direcao.Angulo(), Posicao),
                Posicao
            };

            foreach (var m in meteoros)
            {
                foreach (var col in Colisao)
                {
                    var dist = Vector2.Distance(m.Posicao, col);
                    if (dist < m.Raio)
                    {
                        Visible = false;
                    }
                }

            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Globals.SpriteBatch.Draw(Textura, Posicao, null, Color.White, -Direcao.Angulo(), new Vector2(Textura.Width / 2, Textura.Width / 2), 1f, SpriteEffects.FlipVertically, 0);

            Colisao.ForEach(p =>
            {
                Globals.SpriteBatch.DrawPoint(p, 3, Color.Red);
            });
        }
    }
}