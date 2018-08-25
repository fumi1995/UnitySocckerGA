using UnityEngine;

public static class AnimatorExtension{
    /// <summary>
    /// Check Animation finish
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="name">
    /// target animation clip
    /// </param>
    /// <returns>
    /// true is finish
    /// </returns>
    public static bool CheckEndAnimation(this Animator anim,string name)
    {
        return anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1
            && anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    /// <summary>
    /// Check Animation finish
    /// </summary>
    /// <param name="anim"></param>
    /// <returns>
    /// true is finish
    /// </returns>
    public static bool CheckEndAnimation(this Animator anim)
    {
        return anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;
    }

    /// <summary>
    /// Check Animation over time
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="name">
    ///  target animation clip
    /// </param>
    /// <param name="overTime">
    ///  over animation time which compare normalized time
    /// </param>
    /// <returns>
    ///  true is over time
    /// </returns>
    public static bool CheckOverTimeAnimation(this Animator anim,string name, float overTime)
    {
        return anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= overTime
            && anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    /// <summary>
    /// Check Animation over time
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="overTime">
    /// over animation time which compare normalized time
    /// </param>
    /// <returns>
    /// true is over time
    /// </returns>
    public static bool CheckOverTimeAnimation(this Animator anim,float overTime)
    {
        return anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= overTime;
    }
}
