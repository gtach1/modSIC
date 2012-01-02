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
using System.Linq;
using System.Text;

namespace Modulo.Collect.OVAL.Common
{
    /// <summary>
    /// Helper Class to provide basic OVAL / XML handling facilities
    /// </summary>
    public static class DocumentHelpers
    {
        /// <summary>
        /// Supported OVAL Language / Schema Version
        /// </summary>
        public const decimal oval_schema_version = 5.6M;

        /// <summary>
        /// Creates and fill a "Generator" element for OVAL documents
        /// </summary>
        public static GeneratorType GetDefaultGenerator()
        {
            var newGenerator = new GeneratorType();
            newGenerator.timestamp = DateTime.Now;
            newGenerator.product_name = Resource1.ProductName ;
            var appAssembly = System.Reflection.Assembly.GetExecutingAssembly().GetName();
            newGenerator.product_version = appAssembly.Version.ToString();
            newGenerator.schema_version = oval_schema_version;
            return newGenerator;
        }

        /// <summary>
        /// Adds a new Message To the List
        /// </summary>
        public static List<MessageType> AddMessage(this List<MessageType> messages, MessageLevelEnumeration mLevel, string mText)
        {
            if (messages == null)
                messages = new List<MessageType>();

            messages.Add(new MessageType() { level = mLevel, Value = mText });

            return messages;
        }
    }
}