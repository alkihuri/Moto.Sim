
[System.Serializable]
public class SettingItem
{ 
    public float value; 
    public string definition;

    public SettingItem(float value, string definition)
    {
        this.value = value;
        this.definition = definition;
    }

    public float Value { get => value; set => this.value = value; }
    public string Definition { get => definition; set => definition = value; }
     
}
