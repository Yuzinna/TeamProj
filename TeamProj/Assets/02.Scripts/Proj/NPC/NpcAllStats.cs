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
			entity.splineAnimate.Pause();
			entity.anim.SetBool("IsWalk", false);
		}

		public override void Exit(NPC entity)
		{

		}

		public override void UpdateState(NPC entity)
		{
			//멈춘 상태에서 막힌게 아니라면
			if (entity.isBlocked == false && entity.splineAnimate.ElapsedTime <= entity.splineAnimate.Duration)
			{
				entity.ChangeState(eNpcState.WalkToDesk);
			}
			//Debug.Log($"curSpline: {entity.curSpline}");

			//지금 제출해야하는 상태면
			if (entity.isSubmit == true)
			{
				
				if (entity.curSubmit == eSubmit.Passport)
				{
					entity.curSubmit = eSubmit.None;
					entity.isSubmit = false;
					//전에 여권을 제출했었다면..
					if (entity.wasSubmitPassport)
					{
						return;
					}
					entity.anim.SetTrigger("Passport");
					entity.wasSubmitPassport = true;
				}
				else if (entity.curSubmit == eSubmit.Luggage)
				{
					entity.curSubmit = eSubmit.None;
					entity.isSubmit = false;
					//전에 수하물을 제출했었다면..
					if (entity.wasSubmitLuggage)
					{
						return;
					}
					entity.anim.SetTrigger("Luggage");
					//몸통 돌리기
					entity.transform.DOLocalRotate(new Vector3(0f, 80f, 0f), 2f, RotateMode.LocalAxisAdd)
				.OnComplete(() =>
				{
					entity.transform.DOLocalRotate(new Vector3(0f, -80f, 0f), 2f, RotateMode.LocalAxisAdd);
				});
					entity.wasSubmitLuggage	 = true;
				}
			}

		}
	}
	[System.Serializable]
	public class WalkToDesk : State
	{

		public override void Enter(NPC entity)
		{
			entity.anim.SetBool("IsWalk", true);
			entity.splineAnimate.Play();
		}

		public override void Exit(NPC entity)
		{

		}

		public override void UpdateState(NPC entity)
		{
			//끝지점의 x좌표와 z좌표가 npc의 x와 z좌표의 차가 극도로 0에 가까울때 
			//if ( Mathf.Abs(entity.gameObject.transform.position.x - entity.curSpline.EvaluatePosition(1).x) <= 0.01f
			//	&& Mathf.Abs(entity.gameObject.transform.position.z - entity.curSpline.EvaluatePosition(1).z) <= 0.01f)
			//{
			//	entity.ChangeState(eNpcState.Idle);
			//}
			if (entity.isBlocked == true && entity.splineAnimate.ElapsedTime <= entity.splineAnimate.Duration)//걷는 상태에서 막혔다면
			{
				entity.ChangeState(eNpcState.Idle);
			}
			//Debug.Log($"curSpline: {entity.curSpline}");
			//여기서는 애니메이션이 끝나면 자동으로 제출 애니메이션 재생
			if (entity.splineAnimate.ElapsedTime >= entity.splineAnimate.Duration && entity.currentState.state == eNpcState.WalkToDesk
				&&!entity.isFinSpline)
			{
				entity.isFinSpline = true;
				//Debug.Log("finish!");
				//entity.anim.SetTrigger("Submit");
				entity.ChangeState(eNpcState.Idle);
			}


			//끝지점의 x좌표와 z좌표가 npc의 x와 z좌표의 차가 극도로 0에 가까울때 
			//if (Mathf.Abs(entity.gameObject.transform.position.x - entity.curSpline.EvaluatePosition(1).x) <= 0.01f
			//	&& Mathf.Abs(entity.gameObject.transform.position.z - entity.curSpline.EvaluatePosition(1).z) <= 0.01f)
			//{
			//	Debug.Log("finish!");
			//	entity.anim.SetTrigger("Submit");
			//	entity.ChangeState(eNpcState.Idle);
			//}
		}
	}
	[System.Serializable]
	public class WalkToEnter : State
	{

		public override void Enter(NPC entity)
		{
			entity.curSpline = SplineManager.Instance.splineList[1];
			entity.splineAnimate.Container = entity.curSpline;
			entity.splineAnimate.Restart(true);
			entity.anim.SetBool("IsWalk", true);
			entity.isFinSpline = false;
		}

		public override void Exit(NPC entity)
		{

		}

		public override void UpdateState(NPC entity)
		{
			if (entity.splineAnimate.ElapsedTime >= entity.splineAnimate.Duration && !entity.isFinSpline)
			{
				entity.isFinSpline = true;
				Debug.Log("finishToEnter!");
				
				RuleManager.curNpc -= 1;
				RuleManager.Instance.displayScoreText();
				entity.gameObject.SetActive(false);

			}
		}
	}
	[System.Serializable]
	public class WalkToExit : State
	{

		public override async void Enter(NPC entity)
		{
			entity.curSpline = SplineManager.Instance.splineList[2];
			entity.splineAnimate.Container = entity.curSpline;
			entity.splineAnimate.Restart(true);
			entity.splineAnimate.Pause();

			entity.anim.SetTrigger("HeadNo");
			await Task.Delay(1800);
			entity.splineAnimate.Play();
			entity.isFinSpline = false;

			entity.anim.SetBool("IsWalk", true);
		}


		public override void Exit(NPC entity)
		{

		}

		public override void UpdateState(NPC entity)
		{
			if (entity.splineAnimate.ElapsedTime >= entity.splineAnimate.Duration && !entity.isFinSpline)
			{
				entity.isFinSpline = true;
				Debug.Log("finishToExit!");
				entity.gameObject.SetActive(false);
				RuleManager.curNpc -= 1;
				
				RuleManager.Instance.displayScoreText();
			}
		}
	}
	public class WalkWithPolice : State
	{

		
		public override async void Enter(NPC entity)
		{
			//경찰등장 현재npc가 있는곳까지 
			//경찰은 npc뒤에서 스플라인을 따라감
			
			entity.curSpline = SplineManager.Instance.splineList[4];
			entity.splineAnimate.Container = entity.curSpline;

			
			entity.splineAnimate.Restart(true);
			entity.splineAnimate.Pause();
			
			entity.anim.SetTrigger("HeadNo");
			await Task.Delay(3000);
			entity.splineAnimate.Play();
			entity.anim.SetBool("IsWalk", true);
			entity.isFinSpline = false;

		}

		public override void Exit(NPC entity)
		{
		
		}

		public override void UpdateState(NPC entity)
		{
			if (entity.splineAnimate.ElapsedTime >= entity.splineAnimate.Duration && !entity.isFinSpline)
			{
				entity.isFinSpline = true;
				Debug.Log("finishToPolice!");
				entity.gameObject.SetActive(false);
				RuleManager.curNpc -= 1;
				
				RuleManager.Instance.displayScoreText();
			}
		}
	}
}