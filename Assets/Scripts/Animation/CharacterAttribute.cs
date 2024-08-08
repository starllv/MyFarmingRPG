[System.Serializable]
public struct CharacterAttribute
{
    public CharacterPartAnimator characterPart;
    public PartVariantColour partVariantColour;
    public PartVariantType partVariantType;

    public CharacterAttribute(CharacterPartAnimator characterPart, PartVariantColour partVariantColour, PartVariantType partVariantType) {

        this.characterPart = characterPart;
        this.partVariantColour = partVariantColour;
        this.partVariantType = partVariantType;
    }
}
