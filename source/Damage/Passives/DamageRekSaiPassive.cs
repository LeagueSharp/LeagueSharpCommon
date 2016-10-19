// <copyright file="DamageRekSaiPassive.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Passives
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     RekSai's Damage Passive.
    /// </summary>
    [Export(typeof(IPassiveDamage))]
    [ExportMetadata("ChampionName", "RekSai")]
    public class DamageRekSaiPassive : IPassiveDamage
    {
        #region Public Methods and Operators

        /// <inheritdoc />
        public double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.GetSpellDamage(target, SpellSlot.Q);
        }

        /// <inheritdoc />
        public bool IsActive(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.HasBuff("RekSaiq");
        }

        #endregion
    }
}