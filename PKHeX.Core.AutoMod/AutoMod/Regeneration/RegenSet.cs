﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PKHeX.Core.AutoMod
{
    public class RegenSet
    {
        public static readonly RegenSet Default = new RegenSet(Array.Empty<string>(), PKX.Generation);

        public RegenSetting Extra { get; }
        public ITrainerInfo? Trainer { get; }
        public StringInstructionSet Batch { get; }

        public readonly bool HasExtraSettings;
        public readonly bool HasTrainerSettings;
        public bool HasBatchSettings => Batch.Filters.Count != 0 || Batch.Instructions.Count != 0;

        public RegenSet(PKM pk) : this(Array.Empty<string>(), pk.Format)
        {
            Extra.Ball = (Ball)pk.Ball;
            Extra.ShinyType = pk.ShinyXor == 0 ? Shiny.AlwaysSquare : pk.IsShiny ? Shiny.AlwaysStar : Shiny.Never;
        }

        public RegenSet(ICollection<string> lines, int format, Shiny shiny = Shiny.Never)
        {
            Extra = new RegenSetting {ShinyType = shiny};
            HasExtraSettings = Extra.SetRegenSettings(lines);
            HasTrainerSettings = RegenUtil.GetTrainerInfo(lines, format, out var tr);
            Trainer = tr;
            Batch = new StringInstructionSet(lines);
        }

        public string GetSummary()
        {
            var sb = new StringBuilder();
            if (HasExtraSettings)
                sb.AppendLine(RegenUtil.GetSummary(Extra));
            if (HasTrainerSettings && Trainer != null)
                sb.AppendLine(RegenUtil.GetSummary(Trainer));
            if (HasBatchSettings)
                sb.AppendLine(RegenUtil.GetSummary(Batch));
            return sb.ToString();
        }
    }
}
