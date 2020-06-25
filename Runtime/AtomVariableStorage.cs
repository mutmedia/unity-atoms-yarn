using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Yarn.Unity;
using UnityEngine.Serialization;
using UnityAtoms.BaseAtoms;
using UnityAtoms;

/// An extremely simple implementation of DialogueUnityVariableStorage, which
/// just stores everything in a Dictionary.
namespace UnityAtomsYarn
{
  public class AtomVariableStorage : VariableStorageBehaviour
  {
    public AtomCollection variables;

    /// Reset to our default values when the game starts
    void Awake()
    {
      ResetToDefaults();
    }

    /// Erase all variables and reset to default values
    public override void ResetToDefaults()
    {
      Clear();
      foreach (var kvp in variables.Value)
      {
        kvp.Value.Reset();
      }
    }

    /// Set a variable's value
    public override void SetValue(string variableName, Yarn.Value value)
    {
      variableName = variableName.Substring(1);
      var variable = variables.Value.Get<AtomBaseVariable>(variableName);

      if (variable == null)
      {
        switch (value.type)
        {
          case Yarn.Value.Type.Number:
            {
              var numValue = ScriptableObject.CreateInstance<FloatVariable>();
              numValue.Value = value.AsNumber;
              variables.Value.Add(variableName, numValue);
              break;
            }
          case Yarn.Value.Type.Bool:
            {
              var numValue = ScriptableObject.CreateInstance<BoolVariable>();
              numValue.Value = value.AsBool;
              variables.Value.Add(variableName, numValue);
              break;
            }
          default:
            {
              var numValue = ScriptableObject.CreateInstance<StringVariable>();
              numValue.Value = value.AsString;
              variables.Value.Add(variableName, numValue);
              break;
            }
        }
      }
      else
      {
        variable.BaseValue = value;
      }
    }

    /// Get a variable's value
    public override Yarn.Value GetValue(string variableName)
    {
      // If we don't have a variable with this name, return the null value
      variableName = variableName.Substring(1);
      var variable = variables.Value.Get<AtomBaseVariable>(variableName);
      if (variable == null)
        return Yarn.Value.NULL;

      return new Yarn.Value(variable.BaseValue);
    }

    /// Erase all variables
    public override void Clear()
    {
      variables.Value.Clear();
    }
  }
}