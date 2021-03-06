// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

// Warning: This code was generated by a tool.
// 
// Changes to this file may cause incorrect behavior and will be lost if the
// code is regenerated.

using System;
using System.Linq;
using Microsoft.Azure.Management.Automation.Models;

namespace Microsoft.Azure.Management.Automation.Models
{
    /// <summary>
    /// The parameters supplied to the patch dsc node operation.
    /// </summary>
    public partial class DscNodePatchParameters
    {
        private Guid _id;
        
        /// <summary>
        /// Optional. Gets or sets the id of the dsc node.
        /// </summary>
        public Guid Id
        {
            get { return this._id; }
            set { this._id = value; }
        }
        
        private DscNodeConfigurationAssociationProperty _nodeConfiguration;
        
        /// <summary>
        /// Optional. Gets or sets the configuration of the node.
        /// </summary>
        public DscNodeConfigurationAssociationProperty NodeConfiguration
        {
            get { return this._nodeConfiguration; }
            set { this._nodeConfiguration = value; }
        }
        
        /// <summary>
        /// Initializes a new instance of the DscNodePatchParameters class.
        /// </summary>
        public DscNodePatchParameters()
        {
        }
    }
}
