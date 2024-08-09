using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOverrides : MonoBehaviour
{
    [SerializeField] private GameObject character = null;                 // 保存角色，序列化域，可在Inspector窗口设置
    [SerializeField] private SO_AnimationType[] soAnimationTypeArray = null;   // 用于存放动画的数组

    private Dictionary<AnimationClip, SO_AnimationType> animationTypeDictionaryByAnimation;        // 用于存放动画的字典，键是动画片段
    private Dictionary<string, SO_AnimationType> animationTypeDictionaryByCompositeAttributeKey;   // 键是组合的字符串
    
    private void Start() {
        // 初始化两个字典
        animationTypeDictionaryByAnimation = new Dictionary<AnimationClip, SO_AnimationType>();

        foreach (SO_AnimationType item in soAnimationTypeArray) {

            animationTypeDictionaryByAnimation.Add(item.animationClip, item);
        }

        animationTypeDictionaryByCompositeAttributeKey = new Dictionary<string, SO_AnimationType>();

        foreach (SO_AnimationType item in soAnimationTypeArray) {

            string key = item.characterPart.ToString() + item.partVariantColour.ToString() + item.partVariantType.ToString() + item.animationName.ToString();
            animationTypeDictionaryByCompositeAttributeKey.Add(key, item);
        }
    }

    public void ApplyCharacterCustomisationParameters(List<CharacterAttribute> characterAttributesList) {
        // 遍历角色参数列表，可能要更改角色动画的部分有多个
        foreach (CharacterAttribute characterAttribute in characterAttributesList) {
            // 获取要改变动画的部位的动画控制器
            Animator currentAnimator = null;
            // 初始化动画交换的键值对，用于后面AnimatorOverrideController的使用
            List<KeyValuePair<AnimationClip, AnimationClip>> animsKeyValuePairList = new List<KeyValuePair<AnimationClip, AnimationClip>>();

            string animationSOAssetName = characterAttribute.characterPart.ToString();
            // 获取到角色身上各部位的动画控制器
            Animator[] animatorsArray = character.GetComponentsInChildren<Animator>();
            // 遍历找到指定部位的动画控制器
            foreach (Animator animator in animatorsArray) {

                if (animator.name == animationSOAssetName) {

                    currentAnimator = animator;
                    break;
                }
            }
            // AnimatorOverrideController用于在运行时改变动画
            AnimatorOverrideController aoc = new AnimatorOverrideController(currentAnimator.runtimeAnimatorController);
            // 获取到当前动画控制器中的动画片段
            List<AnimationClip> animationsList = new List<AnimationClip>(aoc.animationClips);

            foreach (AnimationClip animationClip in animationsList) {

                SO_AnimationType so_AnimationType;
                bool foundAnimation = animationTypeDictionaryByAnimation.TryGetValue(animationClip, out so_AnimationType);

                if (foundAnimation) {

                    string key = characterAttribute.characterPart.ToString() + characterAttribute.partVariantColour.ToString() +
                        characterAttribute.partVariantType.ToString() + so_AnimationType.animationName.ToString();

                    SO_AnimationType swapSO_AnimationType;
                    bool foundSwapAnimation = animationTypeDictionaryByCompositeAttributeKey.TryGetValue(key, out swapSO_AnimationType);

                    if (foundSwapAnimation) {

                        AnimationClip swapAnimationClip = swapSO_AnimationType.animationClip;

                        animsKeyValuePairList.Add(new KeyValuePair<AnimationClip, AnimationClip>(animationClip, swapAnimationClip));
                    }
                }
            }

            aoc.ApplyOverrides(animsKeyValuePairList);
            currentAnimator.runtimeAnimatorController = aoc;
        }
    }
}
