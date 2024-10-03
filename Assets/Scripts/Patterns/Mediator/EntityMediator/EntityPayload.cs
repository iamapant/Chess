using System;
using System.Collections.Generic;

public class EntityPayload : Payload<Action<IVisitable>> {
    public override Action<IVisitable> Content { get; set; }

    public override void Visit<T>(T visitable) => visitable.Accept(this);
}