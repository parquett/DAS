﻿using System.Collections.Generic;

namespace Lib.Plugins
{
    /// <summary>
    /// Plugin finder
    /// </summary>
    public interface IPluginFinder
    {
        /// <summary>
        /// Check whether the plugin is available for a certain client
        /// </summary>
        /// <param name="pluginDescriptor">Plugin descriptor to check</param>
        /// <param name="clientId">Store identifier to check</param>
        /// <returns>true - available; false - no</returns>
        bool AuthenticateClient(PluginDescriptor pluginDescriptor, int clientId);

        /// <summary>
        /// Gets plugins
        /// </summary>
        /// <typeparam name="T">The type of plugins to get.</typeparam>
        /// <param name="installedOnly">A value indicating whether to load only installed plugins</param>
        /// <param name="clientId">Load records allowed only in a specified store; pass 0 to load all records</param>
        /// <returns>Plugins</returns>
        IEnumerable<T> GetPlugins<T>(bool installedOnly = true, int clientId = 0) where T : class, IPlugin;

        /// <summary>
        /// Get plugin descriptors
        /// </summary>
        /// <param name="installedOnly">A value indicating whether to load only installed plugins</param>
        /// <param name="clientId">Load records allowed only in a specified store; pass 0 to load all records</param>
        /// <returns>Plugin descriptors</returns>
        IEnumerable<PluginDescriptor> GetPluginDescriptors(bool installedOnly = true, int clientId = 0);

        /// <summary>
        /// Get plugin descriptors
        /// </summary>
        /// <typeparam name="T">The type of plugin to get.</typeparam>
        /// <param name="installedOnly">A value indicating whether to load only installed plugins</param>
        /// <param name="clientId">Load records allowed only in a specified store; pass 0 to load all records</param>
        /// <returns>Plugin descriptors</returns>
        IEnumerable<PluginDescriptor> GetPluginDescriptors<T>(bool installedOnly = true, int clientId = 0) where T : class, IPlugin;

        /// <summary>
        /// Get a plugin descriptor by its system name
        /// </summary>
        /// <param name="systemName">Plugin system name</param>
        /// <param name="installedOnly">A value indicating whether to load only installed plugins</param>
        /// <returns>>Plugin descriptor</returns>
        PluginDescriptor GetPluginDescriptorBySystemName(string systemName, bool installedOnly = true);

        /// <summary>
        /// Get a plugin descriptor by its system name
        /// </summary>
        /// <typeparam name="T">The type of plugin to get.</typeparam>
        /// <param name="systemName">Plugin system name</param>
        /// <param name="installedOnly">A value indicating whether to load only installed plugins</param>
        /// <returns>>Plugin descriptor</returns>
        PluginDescriptor GetPluginDescriptorBySystemName<T>(string systemName, bool installedOnly = true) where T : class, IPlugin;

        /// <summary>
        /// Reload plugins
        /// </summary>
        void ReloadPlugins();
    }
}
