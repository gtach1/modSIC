﻿/*
 * Modulo Open Distributed SCAP Infrastructure Collector (modSIC)
 * 
 * Copyright (c) 2011, Modulo Solutions for GRC.
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 * 
 * - Redistributions of source code must retain the above copyright notice,
 *   this list of conditions and the following disclaimer.
 *   
 * - Redistributions in binary form must reproduce the above copyright 
 *   notice, this list of conditions and the following disclaimer in the
 *   documentation and/or other materials provided with the distribution.
 *   
 * - Neither the name of Modulo Security, LLC nor the names of its
 *   contributors may be used to endorse or promote products derived from
 *   this software without specific  prior written permission.
 *   
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 * */
using System;
using System.Collections.Generic;
using Modulo.Collect.OVAL.SystemCharacteristics;
using Modulo.Collect.OVAL.SystemCharacteristics.Unix;
using Modulo.Collect.Probe.Common;
using Modulo.Collect.Probe.Common.BasicClasses;
using Modulo.Collect.Probe.Common.Helpers;
using Modulo.Collect.Probe.Unix.SSHCollectors;

namespace Modulo.Collect.Probe.Unix.Password
{
    public class PasswordObjectCollector : BaseObjectCollector
    {
        public PasswordCollector PasswordItemsCollector { get; set; }

        public override IList<string> GetValues(Dictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<CollectedItem> collectDataForSystemItem(ItemType systemItem)
        {
            CreatePasswordItemsCollectorInstance();
            var passwordItem = (password_item)systemItem;
            try
            {
                var passwordInfo = this.TryToCollectPasswordItem(passwordItem.username.Value);
                passwordItem.gcos = OvalHelper.CreateItemEntityWithStringValue(passwordInfo.Gecos);
                passwordItem.group_id = OvalHelper.CreateItemEntityWithIntegerValue(passwordInfo.Gid);
                passwordItem.home_dir = OvalHelper.CreateItemEntityWithStringValue(passwordInfo.HomeDir);
                passwordItem.last_login = new EntityItemIntType() { status = StatusEnumeration.notcollected };
                passwordItem.login_shell = OvalHelper.CreateItemEntityWithStringValue(passwordInfo.Shell);
                passwordItem.password = OvalHelper.CreateItemEntityWithStringValue(passwordInfo.Password);
                passwordItem.user_id = OvalHelper.CreateItemEntityWithIntegerValue(passwordInfo.Uid);
            }
            catch (PasswordItemNotExistsException)
            {
                passwordItem.status = StatusEnumeration.doesnotexist;
            }


            return new ItemTypeHelper().CreateCollectedItemsWithOneItem(systemItem, BuildExecutionLog());
        }

        private PasswdInfo TryToCollectPasswordItem(string variableName)
        {
            PasswdInfo variableValue = null;
            var allPasswordItems = this.PasswordItemsCollector.GetTargetPasswdInfo();
            allPasswordItems.TryGetValue(variableName, out variableValue);

            if (variableValue == null)
                throw new PasswordItemNotExistsException();

            return variableValue;
        }

        private void CreatePasswordItemsCollectorInstance()
        {
            if (this.PasswordItemsCollector == null)
                this.PasswordItemsCollector = new PasswordCollector();
        }
    }
}
