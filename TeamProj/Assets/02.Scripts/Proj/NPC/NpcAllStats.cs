using System.Net.NetworkInformation;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Splines;
using static UnityEngine.EventSystems.EventTrigger;
using DG.Tweening;
using UnityEngine.WSA;
namespace NpcAllStats
{
	[System.Serializable]
	public class Idle : State
	{

		public override void Enter(NPC entity)
		{
			
		}

		public override void Exit(NPC entity)
		{

		}

		public override void UpdateState(NPC entity)
		{
		}
	}
	[System.Serializable]
	public class WalkToDesk : State
	{

		public override void Enter(NPC entity)
		{
			
		}

		public override void Exit(NPC entity)
		{

		}

		public override void UpdateState(NPC entity)
		{
			
		}
	}
	[System.Serializable]
	public class WalkToEnter : State
	{

		public override void Enter(NPC entity)
		{
			
		}

		public override void Exit(NPC entity)
		{

		}

		public override void UpdateState(NPC entity)
		{
			
		}
	}
	[System.Serializable]
	public class WalkToExit : State
	{

		public override async void Enter(NPC entity)
		{
			
		}


		public override void Exit(NPC entity)
		{

		}

		public override void UpdateState(NPC entity)
		{
		
		}
	}
	public class WalkWithPolice : State
	{

		
		public override async void Enter(NPC entity)
		{
			
		}

		public override void Exit(NPC entity)
		{
		
		}

		public override void UpdateState(NPC entity)
		{
			
		}
	}
}