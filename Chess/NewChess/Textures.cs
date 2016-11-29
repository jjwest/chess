using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Entities;

namespace NewChess
{
   
    class Textures
    {
        private Dictionary<PieceType, Image> blackTextures;
        private Dictionary<PieceType, Image> whiteTextures;

        public Textures()
        {
            blackTextures = new Dictionary<PieceType, Image>();
            whiteTextures = new Dictionary<PieceType, Image>();
        }

        public void AddTexture(PieceType type, Image blackTexture, Image whiteTexture)
        {
            blackTextures.Add(type, blackTexture);
            whiteTextures.Add(type, whiteTexture);
        }

        public void AddTexturesToGrid(Grid g)
        {
            foreach (var entry in blackTextures)
            {
                g.Children.Add(entry.Value);
            }
            foreach (var entry in whiteTextures)
            {
                g.Children.Add(entry.Value);
            }
        }

        public Image GetTexture(GamePiece piece)
        {
            Image texture;
            if (piece.Color == Color.Black)
            {
                if (blackTextures.TryGetValue(piece.Type, out texture))
                    return texture;
                else
                    throw new TextureNotFoundException("Requested texture has not been loaded");
            }
            else if (piece.Color == Color.White)
            {
                if (whiteTextures.TryGetValue(piece.Type, out texture))
                    return texture;
                else
                    throw new TextureNotFoundException("Requested texture has not been loaded");
            }

            throw new TextureNotFoundException("Requested texture has not been loaded");
        }
    }

    public class TextureNotFoundException : Exception
    {
        public TextureNotFoundException()
        {
        }

        public TextureNotFoundException(string message)
            : base(message)
        {
        }

        public TextureNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
