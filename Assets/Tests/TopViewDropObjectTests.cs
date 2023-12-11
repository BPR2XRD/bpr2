using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

[TestFixture]
public class TopViewDropObjectTests
{
    [Test]
    public void SetImageCooldown_ReduceFillAmountOverTime_ResetCooldownAtZero()
    {
        // Arrange
        var topViewDropObjectScriptObject = new GameObject();
        var topViewDropObjectScript = topViewDropObjectScriptObject.AddComponent<TopViewDropObject>();

        // Set up Image and cooldown variables for testing
        var testImage = new GameObject().AddComponent<Image>();
        var cooldown = 5f;
        var isCooldown = true;

        // Act
        topViewDropObjectScript.SetImageCooldown(ref testImage, ref cooldown, ref isCooldown);

        // Assert
        Assert.GreaterOrEqual(testImage.fillAmount, 0f, "Fill amount should not be negative");
        Assert.LessOrEqual(testImage.fillAmount, 1f, "Fill amount should not exceed 1");
        if (testImage.fillAmount <= 0)
        {
            Assert.IsFalse(isCooldown, "Cooldown should be reset when fill amount reaches 0");
        }
    }

    [Test]
    public void OnToggleValueChanged_CoffinToggleSelected_SetsIsCoffinSelectedToTrue()
    {
        // Arrange
        var topViewDropObjectScriptObject = new GameObject();
        var topViewDropObjectScript = topViewDropObjectScriptObject.AddComponent<TopViewDropObject>();
        var coffinToggleObject = new GameObject();
        var coffinToggle = coffinToggleObject.AddComponent<Toggle>();
        coffinToggle.tag = "Coffin";
        coffinToggle.isOn = true;

        // Act
        topViewDropObjectScript.OnToggleValueChanged(coffinToggle);

        // Assert
        Assert.IsTrue(topViewDropObjectScript.isCoffinSelected, "isCoffinSelected should be true after Coffin toggle is selected");
    }

    [Test]
    public void OnToggleValueChanged_BarricadeToggleSelected_SetsIsCoffinSelectedToFalse()
    {
        // Arrange
        var topViewDropObjectScriptObject = new GameObject();
        var topViewDropObjectScript = topViewDropObjectScriptObject.AddComponent<TopViewDropObject>();
        var barricadeToggleObject = new GameObject();
        var barricadeToggle = barricadeToggleObject.AddComponent<Toggle>();
        barricadeToggle.tag = "Barricade";
        barricadeToggle.isOn = true;

        // Act
        topViewDropObjectScript.OnToggleValueChanged(barricadeToggle);

        // Assert
        Assert.IsFalse(topViewDropObjectScript.isCoffinSelected, "isCoffinSelected should be false after Barricade toggle is selected");
    }

}
