using Content.Client.Stylesheets;
using Content.Client.Stylesheets.Stylesheets;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.Chat.UI;

[CommonSheetlet]
public sealed class RunechatSheetlet : Sheetlet<NanotrasenStylesheet>
{
    public const string FontPath = "/Fonts/_Reserve/Grand9KPixelUnicode.otf";  // Reserve edit: Cyrillic Runechat support
    public const string WhisperFontPath = "/Fonts/_Reserve/Grand9KPixelUnicode.otf";  // Reserve edit: Cyrillic Runechat support

    public override StyleRule[] GetRules(NanotrasenStylesheet sheet, object config)
    {
        var font = ResCache.GetResource<FontResource>(FontPath);
        var whisperFont = ResCache.GetResource<FontResource>(WhisperFontPath);

        return
        [
            E<RichTextLabel>()
                .Class(RunechatSpeechBubble.StyleClassNormal)
                .Prop(Label.StylePropertyFont, new VectorFont(font, RunechatSpeechBubble.NormalSize)),  // Reserve edit: Cyrillic Runechat support
            E<RichTextLabel>()
                .Class(RunechatSpeechBubble.StyleClassLoud)
                .Prop(Label.StylePropertyFont, new VectorFont(font, RunechatSpeechBubble.LoudSize)),  // Reserve edit: Cyrillic Runechat support
            E<RichTextLabel>()
                .Class(RunechatSpeechBubble.StyleClassWhisper)
                .Prop(Label.StylePropertyFont, new VectorFont(whisperFont, RunechatSpeechBubble.SmallSize)),  // Reserve edit: Cyrillic Runechat support
        ];
    }
}
