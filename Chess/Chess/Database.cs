using System;
using Entities;

namespace Database
{
	public interface DatabaseInterface
	{
		GameStateEntity GetState();
		void ClearBoard();
		void SetState(GameStateEntity gameState);
	}
}

