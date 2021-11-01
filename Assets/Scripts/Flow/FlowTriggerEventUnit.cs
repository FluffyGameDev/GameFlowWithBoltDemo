using Bolt;
using Ludiq;

[UnitCategory("Events/Flow")]
public class OnFlowTriggerEvent : EventUnit<FlowTrigger>
{
    [DoNotSerialize]
    public ValueInput m_FlowChannelInput { get; private set; }
    [DoNotSerialize]
    public ValueInput m_FlowTriggerInput { get; private set; }


    protected override bool register => false;

    protected override void Definition()
    {
        base.Definition();

        m_FlowChannelInput = ValueInput<FlowChannel>("Flow Channel");
        m_FlowTriggerInput = ValueInput<FlowTrigger>("Flow Trigger");
    }

    public override IGraphElementData CreateData()
    {
        return new FlowEventData();
    }

    public override void StartListening(GraphStack stack)
    {
        base.StartListening(stack);

        GraphReference reference = stack.ToReference();
        FlowChannel flowChannel = Flow.FetchValue<FlowChannel>(m_FlowChannelInput, reference);
        FlowTrigger triggerToFind = Flow.FetchValue<FlowTrigger>(m_FlowTriggerInput, reference);
        FlowEventData data = stack.GetElementData<FlowEventData>(this);

        if (data == null)
            return;

        data.Callback = (FlowTrigger trigger) =>
        {
            if (trigger == triggerToFind)
            {
                Trigger(reference, trigger);
            }
        };

        flowChannel.OnFlowTrigger += data.Callback;
    }

    public override void StopListening(GraphStack stack)
    {
        base.StopListening(stack);

        GraphReference reference = stack.ToReference();
        FlowChannel flowChannel = Flow.FetchValue<FlowChannel>(m_FlowChannelInput, reference);
        FlowEventData data = stack.GetElementData<FlowEventData>(this);

        if (data != null && data.Callback != null)
        {
            flowChannel.OnFlowTrigger -= data.Callback;
        }
    }

    public class FlowEventData : Data
    {
        public FlowChannel.FlowTriggerCallback Callback;
    }
}