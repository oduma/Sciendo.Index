﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sciendo.Indexer.Sciendo.Indexer.Agent.Client {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://Sciendo.Indexer.Agent", ConfigurationName="Sciendo.Indexer.Agent.Client.IIndexerAgent")]
    public interface IIndexerAgent {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Sciendo.Indexer.Agent/IIndexerAgent/IndexLyricsOnDemand", ReplyAction="http://Sciendo.Indexer.Agent/IIndexerAgent/IndexLyricsOnDemandResponse")]
        int IndexLyricsOnDemand(string fromPath);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Sciendo.Indexer.Agent/IIndexerAgent/IndexLyricsOnDemand", ReplyAction="http://Sciendo.Indexer.Agent/IIndexerAgent/IndexLyricsOnDemandResponse")]
        System.Threading.Tasks.Task<int> IndexLyricsOnDemandAsync(string fromPath);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Sciendo.Indexer.Agent/IIndexerAgent/IndexMusicOnDemand", ReplyAction="http://Sciendo.Indexer.Agent/IIndexerAgent/IndexMusicOnDemandResponse")]
        int IndexMusicOnDemand(string fromPath);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Sciendo.Indexer.Agent/IIndexerAgent/IndexMusicOnDemand", ReplyAction="http://Sciendo.Indexer.Agent/IIndexerAgent/IndexMusicOnDemandResponse")]
        System.Threading.Tasks.Task<int> IndexMusicOnDemandAsync(string fromPath);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IIndexerAgentChannel : Sciendo.Indexer.Sciendo.Indexer.Agent.Client.IIndexerAgent, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class IndexerAgentClient : System.ServiceModel.ClientBase<Sciendo.Indexer.Sciendo.Indexer.Agent.Client.IIndexerAgent>, Sciendo.Indexer.Sciendo.Indexer.Agent.Client.IIndexerAgent {
        
        public IndexerAgentClient() {
        }
        
        public IndexerAgentClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public IndexerAgentClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IndexerAgentClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IndexerAgentClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public int IndexLyricsOnDemand(string fromPath) {
            return base.Channel.IndexLyricsOnDemand(fromPath);
        }
        
        public System.Threading.Tasks.Task<int> IndexLyricsOnDemandAsync(string fromPath) {
            return base.Channel.IndexLyricsOnDemandAsync(fromPath);
        }
        
        public int IndexMusicOnDemand(string fromPath) {
            return base.Channel.IndexMusicOnDemand(fromPath);
        }
        
        public System.Threading.Tasks.Task<int> IndexMusicOnDemandAsync(string fromPath) {
            return base.Channel.IndexMusicOnDemandAsync(fromPath);
        }
    }
}