using System;
using System.Collections.Generic;

namespace Mediator.EntityMediator {
    public class EntityPayload : Payload<Action<IVisitable>> {
        public override Action<IVisitable> Content { get; set; }

        public override void Visit<T>(T visitable) {
            var entity = visitable as Entity;
            var square = visitable as Square;

            if (square != null) {
                VisitSquare(square);
            }

            if (visitable) {
                VisitEntity(entity);
            }
        }

        private void VisitEntity(Entity entity) {
            entity.Accept(this);
        }

        private void VisitSquare(Square square) {
            square.Accept(this);
        }
    }
}