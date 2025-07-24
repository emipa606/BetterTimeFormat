using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace BetterTimeFormat;

[HarmonyPatch(typeof(DateReadout), nameof(DateReadout.DateOnGUI))]
internal static class DateReadout_DateOnGUI
{
    public static bool Prefix(Rect dateRect, ref List<string> ___seasonsCached, ref int ___dateStringDay,
        ref Season ___dateStringSeason, ref Quadrum ___dateStringQuadrum, ref int ___dateStringYear,
        ref string ___dateString)
    {
        Vector2 longLat;
        switch (WorldRendererUtility.WorldRenderedNow)
        {
            case true when Find.WorldSelector.selectedTile >= 0:
                longLat = Find.WorldGrid.LongLatOf(Find.WorldSelector.selectedTile);
                break;

            case true when Find.WorldSelector.NumSelectedObjects > 0:
                longLat = Find.WorldGrid.LongLatOf(Find.WorldSelector.FirstSelectedObject.Tile);
                break;

            default:
            {
                if (Find.CurrentMap == null)
                {
                    return false;
                }

                longLat = Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile);
                break;
            }
        }

        var dayOfTwelfth = GenDate.DayOfTwelfth(Find.TickManager.TicksAbs, longLat.x);
        var season = GenDate.Season(Find.TickManager.TicksAbs, longLat);
        var quadrum = GenDate.Quadrum(Find.TickManager.TicksAbs, longLat.x);
        var year = GenDate.Year(Find.TickManager.TicksAbs, longLat.x);
        var seasonCached = ___seasonsCached[(int)season];
        if (dayOfTwelfth != ___dateStringDay || season != ___dateStringSeason || quadrum != ___dateStringQuadrum ||
            year != ___dateStringYear)
        {
            ___dateString = GenDate.DateReadoutStringAt(Find.TickManager.TicksAbs, longLat);
            ___dateStringDay = dayOfTwelfth;
            ___dateStringSeason = season;
            ___dateStringQuadrum = quadrum;
            ___dateStringYear = year;
        }

        var userTime = "";
        if (BetterTimeFormatMod.Settings.UpdateTime)
        {
            userTime = BetterTimeFormatMod.Settings.TimeFormat;
            var dayPercent = GenLocalDate.DayPercent(Find.CurrentMap);

            if (BetterTimeFormatMod.Settings.UpdateHours)
            {
                var rawHours = Math.Floor(dayPercent * 24);
                var hours = rawHours;

                if (Prefs.TwelveHourClockMode)
                {
                    hours %= 12;
                    if (hours == 0)
                        hours = 12;
                }

                userTime = userTime.ReplaceFirst("HH", $"{hours:00}");
                userTime = userTime.ReplaceFirst("H", $"{hours:0}");
            }

            if (BetterTimeFormatMod.Settings.UpdateMinutes)
            {
                var minutes = Math.Floor(dayPercent * 24 % 1 * 60);
                userTime = userTime.ReplaceFirst("MM", $"{minutes:00}");
                userTime = userTime.ReplaceFirst("M", $"{minutes:0}");
            }

            if (BetterTimeFormatMod.Settings.UpdateSeconds)
            {
                var seconds = Math.Floor(dayPercent * 24 % 1 * 60 % 1 * 60);
                userTime = userTime.ReplaceFirst("SS", $"{seconds:00}");
                userTime = userTime.ReplaceFirst("S", $"{seconds:0}");
            }

            if (Prefs.TwelveHourClockMode)
            {
                var notation = dayPercent < 0.5
                    ? BetterTimeFormatMod.Settings.AmString
                    : BetterTimeFormatMod.Settings.PmString;
                userTime = userTime.ReplaceFirst("N", notation);
            }
        }

        Text.Font = GameFont.Small;
        var width =
            Mathf.Max(Mathf.Max(Text.CalcSize(userTime).x, Text.CalcSize(___dateString).x),
                Text.CalcSize(seasonCached).x) + 7f;
        dateRect.xMin = dateRect.xMax - width;
        if (Mouse.IsOver(dateRect))
        {
            Widgets.DrawHighlight(dateRect);
        }

        GUI.BeginGroup(dateRect);
        Text.Font = GameFont.Small;
        Text.Anchor = TextAnchor.UpperRight;
        var rect = dateRect.AtZero();
        rect.xMax -= 7f;
        Widgets.Label(rect, userTime);
        rect.yMin += 26f;
        Widgets.Label(rect, ___dateString);
        rect.yMin += 26f;
        if (!seasonCached.NullOrEmpty())
        {
            Widgets.Label(rect, seasonCached);
        }

        Text.Anchor = TextAnchor.UpperLeft;
        GUI.EndGroup();
        if (!Mouse.IsOver(dateRect))
        {
            return false;
        }

        var stringBuilder = new StringBuilder();
        for (var i = 0; i < 4; ++i)
        {
            var q = (Quadrum)i;
            stringBuilder.AppendLine($"{q.Label()} - {q.GetSeason(longLat.y).LabelCap()}");
        }

        var tip = "DateReadoutTip".Translate(GenDate.DaysPassed, 15, (NamedArgument)season.LabelCap(), 15,
            (NamedArgument)GenDate.Quadrum(GenTicks.TicksAbs, longLat.x).Label(),
            (NamedArgument)stringBuilder.ToString());
        TooltipHandler.TipRegion(dateRect, new TipSignal(tip, 86423));

        return false;
    }
}
