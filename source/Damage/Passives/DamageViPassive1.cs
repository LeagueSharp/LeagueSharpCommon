// <copyright file="DamageViPassive1.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Passives
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Vi's Damage Passive.
    /// </summary>
    [Export(typeof(IPassiveDamage))]
    [ExportMetadata("ChampionName", "Vi")]
    public class DamageViPassive1 : IPassiveDamage
    {
        #region Public Methods and Operators

        /// <inheritdoc />
        public double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.GetSpellDamage(target, SpellSlot.W);
        }

        /// <inheritdoc />
        public bool IsActive(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.HasBuff("ViE");
        }

        #endregion
    }
}