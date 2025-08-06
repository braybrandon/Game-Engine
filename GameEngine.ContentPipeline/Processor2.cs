using Common.Enums;
using GameEngine.ContentPipeline;
using GameEngine.IO.Asset.models;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

[ContentProcessor(DisplayName = "Keybind Processor")]
public class KeybindProcessor : ContentProcessor<KeybindRaw, Keybinds>
{
    public override Keybinds Process(KeybindRaw input, ContentProcessorContext context)
    {
        var keyBinds = new Keybinds();
        foreach (var kvp in input.Movement)
        {
            var key = (Keys)Enum.Parse(typeof(Keys), kvp.Value);
            var action = (InputAction)Enum.Parse(typeof(InputAction), kvp.Key);
            keyBinds.Movement[key] = action;
        }
        foreach (var kvp in input.Attack)
        {
            var key = (Keys)Enum.Parse(typeof(Keys), kvp.Value);
            var action = (InputAction)Enum.Parse(typeof(InputAction), kvp.Key);
            keyBinds.Attack[key] = action;
        }
        return keyBinds;
    }
}