using System;
using System.ComponentModel;
using System.Reflection;

namespace TicTacToe
{
    //TODO: Rename this
    public enum FieldState
    {
        Empty = 0,
        [Description("X")]
        PlayerX = 1,
        [Description("O")]
        PlayerO = -1
    }

    public class Field
    {
        public FieldState State;
        public int Round;
        public char Symbol;

        public Field(char sym)
        {
            Round = -1;
            State = FieldState.Empty;
            Symbol = sym;
        }
    }
    
    public static class FieldStateHelper
    {
        public static string GetDescription(this Enum value){
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr = 
                        Attribute.GetCustomAttribute(field, 
                            typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }
    }
}
