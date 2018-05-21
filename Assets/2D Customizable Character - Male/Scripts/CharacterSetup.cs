using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CharacterSetup : MonoBehaviour {
	[Header("Direction Game Objects")]
    public GameObject downDirection;
    public GameObject upDirection;
    public GameObject leftDirection;
    public GameObject rightDirection;
	[Header("Weapons")]
	public Sprite swingWeapon;
	public Sprite thrustWeapon;
	public Sprite bowWeapon;
	[Header("Base")]
    public Color skinColor;
	public Sprite eyeShape;
    public Color eyeColor;
	[Header("Hair")]
	public Sprite hair;
    public Sprite facialHair;
	public Sprite eyebrowShape;
	public Color hairColor;
	[Header("Headwear")]
    public Sprite headwear;
    public Color headwearColor;
	[Header("Shoulders")]
	public Sprite leftShoulder;
	public Color leftShoulderColor;
	[Space(5)] 
	public Sprite rightShoulder;
	public Color rightShoulderColor;
	[Header("Left Arm")]
    public Sprite leftUpperArmEquipment;
    public Color leftUpperArmEquipmentColor;
	[Space(5)] 
	public Sprite leftLowerArmEquipment;
	public Color leftLowerArmEquipmentColor;
	[Space(5)] 
	public Sprite leftHandEquipment;
	public Color leftHandEquipmentColor;
	[Header("Right Arm")]
	public Sprite rightUpperArmEquipment;
	public Color rightUpperArmEquipmentColor;
	[Space(5)] 
	public Sprite rightLowerArmEquipment;
	public Color rightLowerArmEquipmentColor;
	[Space(5)] 
	public Sprite rightHandEquipment;
	public Color rightHandEquipmentColor;
	[Header("Upper Body")]
    public Sprite chestEquipment;
    public Color chestEquipmentColor;
	[Space(5)] 
    public Sprite bodyEquipment;
    public Color bodyEquipmentColor;
	[Space(5)] 
    public Sprite belt;
    public Color beltColor;
	[Space(5)] 
	public Sprite hipEquipment;
	public Color hipEquipmentColor;
	[Header("Left Leg")]
	public Sprite leftUpperLegEquipment;
	public Color leftUpperLegEquipmentColor;
	[Space(5)] 
	public Sprite leftLowerLegEquipment;
	public Color leftLowerLegEquipmentColor;
	[Space(5)] 
	public Sprite leftFootEquipment;
	public Color leftFootEquipmentColor;
	[Header("Right Leg")]
    public Sprite rightUpperLegEquipment;
    public Color rightUpperLegEquipmentColor;
	[Space(5)] 
    public Sprite rightLowerLegEquipment;
    public Color rightLowerLegEquipmentColor;
	[Space(5)] 
    public Sprite rightFootEquipment;
    public Color rightFootEquipmentColor;

    private GameObject previousDownDirection;

    private SpriteRenderer[] cachedUpSpriteRenderers = null;
    private SpriteRenderer[] cachedLeftSpriteRenderers = null;
    private SpriteRenderer[] cachedDownSpriteRenderers = null;
    private SpriteRenderer[] cachedRightSpriteRenderers = null;

    void Awake()
    {
        if (downDirection != null)
        {
            UpdateCachedSpriteRenderers(downDirection, ref cachedDownSpriteRenderers);

            if (cachedDownSpriteRenderers != null && cachedDownSpriteRenderers.Length > 0)
            {
				swingWeapon = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__swing weapon slot").sprite;
				thrustWeapon = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__thrust weapon slot").sprite;
				bowWeapon = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__bow weapon slot").sprite;

                skinColor = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "head").color;
				eyeShape = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "eyes").sprite;
				eyebrowShape = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "eyebrows").sprite;
                eyeColor = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "eye color").color;

                hairColor = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__hair slot").color;
                hair = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__hair slot").sprite;
                facialHair = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__facial hair slot").sprite;

                headwear = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__headwear slot").sprite;
                headwearColor = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__headwear slot").color;

                rightShoulder = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__right shoulder slot").sprite;
                rightShoulderColor = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__right shoulder slot").color;
                rightUpperArmEquipment = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__right upper arm slot").sprite;
                rightUpperArmEquipmentColor = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__right upper arm slot").color;
				rightLowerArmEquipment = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__right lower arm slot").sprite;
				rightLowerArmEquipmentColor = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__right lower arm slot").color;
				rightHandEquipment = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__right hand slot").sprite;
				rightHandEquipmentColor = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__right hand slot").color;

                leftShoulder = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__left shoulder slot").sprite;
                leftShoulderColor = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__left shoulder slot").color;
                leftUpperArmEquipment = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__left upper arm slot").sprite;
                leftUpperArmEquipmentColor = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__left upper arm slot").color;
				leftLowerArmEquipment = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__left lower arm slot").sprite;
				leftLowerArmEquipmentColor = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__left lower arm slot").color;
				leftHandEquipment = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__left hand slot").sprite;
				leftHandEquipmentColor = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__left hand slot").color;

                chestEquipment = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__chest slot").sprite;
                chestEquipmentColor = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__chest slot").color;
                bodyEquipment = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__body slot").sprite;
                bodyEquipmentColor = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__body slot").color;
				belt = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__belt slot").sprite;
				beltColor = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__belt slot").color;
                hipEquipment = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__hip slot").sprite;
                hipEquipmentColor = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__hip slot").color;

                rightUpperLegEquipment = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__right upper leg slot").sprite;
                rightUpperLegEquipmentColor = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__right upper leg slot").color;
                rightLowerLegEquipment = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__right lower leg slot").sprite;
                rightLowerLegEquipmentColor = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__right lower leg slot").color;
                rightFootEquipment = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__right foot slot").sprite;
                rightFootEquipmentColor = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__right foot slot").color;			
                
				leftUpperLegEquipment = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__left upper leg slot").sprite;
                leftUpperLegEquipmentColor = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__left upper leg slot").color;	
                leftLowerLegEquipment = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__left lower leg slot").sprite;
                leftLowerLegEquipmentColor = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__left lower leg slot").color;
                leftFootEquipment = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__left foot slot").sprite;
                leftFootEquipmentColor = GetSpriteRendererBySlotName(cachedDownSpriteRenderers, "__left foot slot").color;
            }
        }
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            if (downDirection != previousDownDirection)
            {
                Awake();
                previousDownDirection = downDirection;
            }
            UpdateSprites(true);
        }
    }

    public void UpdateSprites(bool updateColorOnly)
    {
        UpdateCachedSpriteRenderersForAllDirections();

        UpdateSpritesForDirection(cachedDownSpriteRenderers, "down", updateColorOnly);
        UpdateSpritesForDirection(cachedUpSpriteRenderers, "up", updateColorOnly);        
        if (rightDirection != leftDirection && rightDirection != null)
        {
			UpdateSpritesForDirection(cachedLeftSpriteRenderers, "side", updateColorOnly);
			UpdateSpritesForDirection(cachedRightSpriteRenderers, "side", updateColorOnly);
        }
        else
        {
            UpdateSpritesForDirection(cachedLeftSpriteRenderers, "side", updateColorOnly);
        }
    }

    private void UpdateCachedSpriteRenderersForAllDirections()
    {
        UpdateCachedSpriteRenderers(downDirection, ref cachedDownSpriteRenderers);
        UpdateCachedSpriteRenderers(upDirection, ref cachedUpSpriteRenderers);
        UpdateCachedSpriteRenderers(leftDirection, ref cachedLeftSpriteRenderers);
        if (rightDirection != leftDirection)
        {
            UpdateCachedSpriteRenderers(rightDirection, ref cachedRightSpriteRenderers);
        }
    }

    private void UpdateCachedSpriteRenderers(GameObject directionGameObject, ref SpriteRenderer[] cachedSpriteRenderers)
    {
        if (directionGameObject != null)
        {
            if (cachedSpriteRenderers == null || cachedSpriteRenderers.Length == 0)
            {
                cachedSpriteRenderers = directionGameObject.GetComponentsInChildren<SpriteRenderer>(true);
            }
        }
    }

    private void UpdateSpritesForDirection(SpriteRenderer[] cachedSpriteRenderers, string direction, bool updateColorOnly)
    {
        if (cachedSpriteRenderers != null && cachedSpriteRenderers.Length > 0)
        {
            bool isMainDirection = direction == "down";
            bool updateSprites = isMainDirection || !updateColorOnly;

			//weapons
			SpriteRenderer swingWeaponSlotSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__swing weapon slot");
			UpdateSprite(swingWeaponSlotSR, swingWeapon, swingWeaponSlotSR.color, direction, isMainDirection, updateSprites);
			SpriteRenderer thrustWeaponSlotSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__thrust weapon slot");
			UpdateSprite(thrustWeaponSlotSR, thrustWeapon, thrustWeaponSlotSR.color, direction, isMainDirection, updateSprites);
			SpriteRenderer bowWeaponSlotSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__bow weapon slot");
			UpdateSprite(bowWeaponSlotSR, bowWeapon, bowWeaponSlotSR.color, direction, isMainDirection, updateSprites);

			//face
            SpriteRenderer headSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "head");
            headSR.color = skinColor;

			SpriteRenderer eyesClosedSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "eyes closed");
			eyesClosedSR.color = new Color (skinColor.r, skinColor.g, skinColor.b, eyesClosedSR.color.a);

			SpriteRenderer eyebrowShapeSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "eyebrows");
			UpdateSprite(eyebrowShapeSR, eyebrowShape, hairColor, direction, isMainDirection, updateSprites);

		    SpriteRenderer eyesSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "eye color");
		    eyesSR.color = eyeColor;

			SpriteRenderer eyeshapeSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "eyes");
			UpdateSprite(eyeshapeSR, eyeShape, Color.white, direction, isMainDirection, updateSprites);

            //hair and headwear
            SpriteRenderer hairSlotSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__hair slot");
            UpdateSprite(hairSlotSR, hair, hairColor, direction, isMainDirection, updateSprites);

            SpriteRenderer facialHairSlotSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__facial hair slot");
            UpdateSprite(facialHairSlotSR, facialHair, hairColor, direction, isMainDirection, updateSprites);

            SpriteRenderer headwearSlotSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__headwear slot");
            UpdateSprite(headwearSlotSR, headwear, headwearColor, direction, isMainDirection, updateSprites);

            //right arm
            SpriteRenderer rightShoulderSlotSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__right shoulder slot");
            UpdateSprite(rightShoulderSlotSR, rightShoulder, rightShoulderColor, direction, isMainDirection, updateSprites);

            SpriteRenderer rightUpperArmSlotSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__right upper arm slot");
            UpdateSprite(rightUpperArmSlotSR, rightUpperArmEquipment, rightUpperArmEquipmentColor, direction, isMainDirection, updateSprites);

			SpriteRenderer rightLowerArmSlotSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__right lower arm slot");
			UpdateSprite(rightLowerArmSlotSR, rightLowerArmEquipment, rightLowerArmEquipmentColor, direction, isMainDirection, updateSprites);

			SpriteRenderer rightHandSlotSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__right hand slot");
			UpdateSprite(rightHandSlotSR, rightHandEquipment, rightHandEquipmentColor, direction, isMainDirection, updateSprites);

            SpriteRenderer rightUpperArmSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "right upper arm");
			rightUpperArmSR.color = skinColor;
		
            SpriteRenderer rightLowerArmSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "right lower arm");
			rightLowerArmSR.color = skinColor;
	
			SpriteRenderer rightHandSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "right hand");
			rightHandSR.color = skinColor;

            //left arm
            SpriteRenderer leftShoulderSlotSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__left shoulder slot");
            UpdateSprite(leftShoulderSlotSR, leftShoulder, leftShoulderColor, direction, isMainDirection, updateSprites);

            SpriteRenderer leftUpperArmSlotSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__left upper arm slot");
            UpdateSprite(leftUpperArmSlotSR, leftUpperArmEquipment, leftUpperArmEquipmentColor, direction, isMainDirection, updateSprites);

			SpriteRenderer leftLowerArmSlotSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__left lower arm slot");
			UpdateSprite(leftLowerArmSlotSR, leftLowerArmEquipment, leftLowerArmEquipmentColor, direction, isMainDirection, updateSprites);

			SpriteRenderer leftHandSlotSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__left hand slot");
			UpdateSprite (leftHandSlotSR, leftHandEquipment, leftHandEquipmentColor, direction, isMainDirection, updateSprites);

         	SpriteRenderer leftUpperArmSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "left upper arm");
			leftUpperArmSR.color = skinColor;
		
			SpriteRenderer leftLowerArmSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "left lower arm");
			leftLowerArmSR.color = skinColor;
		
			SpriteRenderer leftHandSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "left hand");
			leftHandSR.color = skinColor;

            //chest
            SpriteRenderer chestSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "chest");
			chestSR.color = skinColor;
 
            SpriteRenderer chestSlotSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__chest slot");
            UpdateSprite(chestSlotSR, chestEquipment, chestEquipmentColor, direction, isMainDirection, updateSprites);

            //body
            SpriteRenderer bodySR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "body");
			bodySR.color = skinColor;

            SpriteRenderer bodySlotSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__body slot");
            UpdateSprite(bodySlotSR, bodyEquipment, bodyEquipmentColor, direction, isMainDirection, updateSprites);

			SpriteRenderer beltSlotSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__belt slot");
            UpdateSprite(beltSlotSR, belt, beltColor, direction, isMainDirection, updateSprites);

			SpriteRenderer hipSlotSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__hip slot");
			UpdateSprite(hipSlotSR, hipEquipment, hipEquipmentColor, direction, isMainDirection, updateSprites);

            //right leg
            SpriteRenderer rightUpperLegSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "right upper leg");
			rightUpperLegSR.color = skinColor;

            SpriteRenderer rightUpperLegSlotSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__right upper leg slot");
            UpdateSprite(rightUpperLegSlotSR, rightUpperLegEquipment, rightUpperLegEquipmentColor, direction, isMainDirection, updateSprites);

            SpriteRenderer rightLowerLegSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "right lower leg");
			rightLowerLegSR.color = skinColor;
   
            SpriteRenderer rightLowerLegSlotSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__right lower leg slot");
            UpdateSprite(rightLowerLegSlotSR, rightLowerLegEquipment, rightLowerLegEquipmentColor, direction, isMainDirection, updateSprites);

            SpriteRenderer rightFootSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "right foot");
			rightFootSR.color = skinColor;

            SpriteRenderer rightFootSlotSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__right foot slot");
            UpdateSprite(rightFootSlotSR, rightFootEquipment, rightFootEquipmentColor, direction, isMainDirection, updateSprites);

            //left leg
            SpriteRenderer leftUpperLegSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "left upper leg");
			leftUpperLegSR.color = skinColor;

            SpriteRenderer leftUpperLegSlotSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__left upper leg slot");
            UpdateSprite(leftUpperLegSlotSR, leftUpperLegEquipment, leftUpperLegEquipmentColor, direction, isMainDirection, updateSprites);

            SpriteRenderer leftLowerLegSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "left lower leg");
			leftLowerLegSR.color = skinColor;

            SpriteRenderer leftLowerLegSlotSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__left lower leg slot");
            UpdateSprite(leftLowerLegSlotSR, leftLowerLegEquipment, leftLowerLegEquipmentColor, direction, isMainDirection, updateSprites);

            SpriteRenderer leftFootSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "left foot");
			leftFootSR.color = skinColor;

            SpriteRenderer leftFootSlotSR = GetSpriteRendererBySlotName(cachedSpriteRenderers, "__left foot slot");
            UpdateSprite(leftFootSlotSR, leftFootEquipment, leftFootEquipmentColor, direction, isMainDirection, updateSprites);
        }
    }

    private void UpdateSprite(SpriteRenderer spriteRenderer, Sprite sprite, Color color, string direction, bool isMainDirection, bool updateSprite)
    {
#if UNITY_EDITOR
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;

            if (updateSprite)
            {
                if (isMainDirection)
                {
                    spriteRenderer.sprite = sprite;
                }
                else
                {
                    if (sprite != null)
                    {
                        string spriteName = sprite.name;
                        spriteName = spriteName.Replace("down", direction);
                        spriteName = spriteName.Replace("Down", direction.Substring(0,1).ToUpper() + direction.Substring(1, direction.Length - 1));
                        string[] assets = UnityEditor.AssetDatabase.FindAssets(spriteName);
                        for (int i = 0; i < assets.Length; i++)
                        {
                            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(assets[0]);
                            if (UnityEditor.AssetDatabase.GetMainAssetTypeAtPath(path) == typeof(Sprite))
                            {
                                Sprite loadedSprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(path);
                                spriteRenderer.sprite = sprite;
                                break;
                            }
                            else if (UnityEditor.AssetDatabase.GetMainAssetTypeAtPath(path) == typeof(Texture2D))
                            {
                                bool set = false;
                                Object[] spriteArray = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(path);
                                for (int j = 0; j < spriteArray.Length; j++)
                                {
                                    if (spriteArray[j].GetType() == typeof(Sprite) && spriteArray[j].name == spriteName)
                                    {
                                        spriteRenderer.sprite = spriteArray[j] as Sprite;
                                        set = true;
                                        break;
                                    }
                                }
                                if (set)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
#endif
    }

    private SpriteRenderer GetSpriteRendererBySlotName(SpriteRenderer[] cachedSpriteRenderers, string name)
    {
        for (int i = 0; i < cachedSpriteRenderers.Length; i++)
        {
            if (cachedSpriteRenderers[i].transform.parent.name == name)
            {
                return cachedSpriteRenderers[i];
            }
        }

        return null;
    }
}
