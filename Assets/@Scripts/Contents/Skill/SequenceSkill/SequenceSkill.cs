using System;

public abstract class SequenceSkill : SkillBase
{
    public int DataId;// 보정된 데이터 아이디가 아닌 원본
    public abstract void DoSkill(Action callback = null);
    public string AnimagtionName;
}
