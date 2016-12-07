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
        private Dictionary<PieceType, BitmapImage> blackTextures;
        private Dictionary<PieceType, BitmapImage> whiteTextures;

        public Textures()
        {
            blackTextures = new Dictionary<PieceType, BitmapImage>();
            whiteTextures = new Dictionary<PieceType, BitmapImage>();
        }

        public void AddTexture(PieceType type, BitmapImage blackTexture, BitmapImage whiteTexture)
        {
            blackTextures.Add(type, blackTexture);
            whiteTextures.Add(type, whiteTexture);
        }

        public Image GetTexture(GamePiece piece)
        {
            Image image = new Image();
            BitmapImage bitmap;
            if (piece.Color == Color.Black)
            {
                if (blackTextures.TryGetValue(piece.Type, out bitmap))
                {
                    image.Source = bitmap;
                    return image;
                }
                else
                    throw new TextureNotFoundException("Requested texture has not been loaded");
            }
            else if (piece.Color == Color.White)
            {
                if (whiteTextures.TryGetValue(piece.Type, out bitmap))
                {
                    image.Source = bitmap;
                    return image;
                }
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
