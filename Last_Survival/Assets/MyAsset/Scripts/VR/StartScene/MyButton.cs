using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class MyButton : Button
{
    ///
    /// 配合Unity的其他方法使用，就能达到你想要的效果！这里只是抛砖引玉，大家有更好的方法欢迎跟我交流！
    ///
    ///
    ///
    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);
        switch (state)
        {
            case SelectionState.Disabled:
                break;
			case SelectionState.Highlighted:
				GetComponent<Animation> ().Play ("ButtonTrigger");
                break;
            case SelectionState.Normal:
				GetComponent<Animation> ().Play ("ButtonUntrigger");
                break;
            case SelectionState.Pressed:
                break;
            default:
                break;
        }
    }
}