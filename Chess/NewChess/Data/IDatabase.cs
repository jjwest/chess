using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace Data
{
    public interface DatabaseInterface
    {
        GameStateEntity GetState();
        void ResetBoard();
        void SaveState(GameStateEntity gameState);

        string DatabasePath { get; set; }
    }
}
