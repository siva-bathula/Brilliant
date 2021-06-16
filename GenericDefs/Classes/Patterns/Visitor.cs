using GenericDefs.Util;

namespace GenericDefs.Patterns
{
    /// <summary>
    /// Provides an interface for visitors.
    /// </summary>
    /// <typeparam name="T">The type of objects to be visited.</typeparam>
    public interface IVisitor<T>
    {
        /// <summary>
        /// Gets a value indicating whether this instance is done performing it's work..
        /// </summary>
        /// <value><c>true</c> if this instance is done; otherwise, <c>false</c>.</value>
        bool HasCompleted
        {
            get;
        }

        /// <summary>
        /// Visits the specified object.
        /// </summary>
        /// <param name="obj">The object to visit.</param>
        void Visit(T obj);
    }

    /// <summary>
    /// A visitor that visits objects in order (PreOrder, PostOrder, or InOrder).
    /// Used primarily as a base class for Visitors specializing in a specific order type.
    /// </summary>
    /// <typeparam name="T">The type of objects to be visited.</typeparam>
    public class OrderedVisitor<T> : IVisitor<T>
    {
        private readonly IVisitor<T> visitorToUse;

        /// <summary>
        /// Determines whether this visitor is done.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// 	<c>true</c> if this visitor is done; otherwise, <c>false</c>.
        /// </returns>
        public bool HasCompleted
        {
            get
            {
                return this.visitorToUse.HasCompleted;
            }
        }

        /// <summary>
        /// Gets the visitor to use.
        /// </summary>
        /// <value>The visitor to use.</value>
        public IVisitor<T> VisitorToUse
        {
            get
            {
                return this.visitorToUse;
            }
        }

        /// <param name="visitorToUse">The visitor to use when visiting the object.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitorToUse" /> is a null reference (<c>Nothing</c> in Visual Basic).</exception>
        public OrderedVisitor(IVisitor<T> visitorToUse)
        {
            Guard.ArgumentNotNull(visitorToUse, "visitorToUse");
            this.visitorToUse = visitorToUse;
        }

        /// <summary>
        /// Visits the object in pre order.
        /// </summary>
        /// <param name="obj">The obj.</param>         
        public virtual void VisitPreOrder(T obj)
        {
            this.visitorToUse.Visit(obj);
        }

        /// <summary>
        /// Visits the object in post order.
        /// </summary>
        /// <param name="obj">The obj.</param>        
        public virtual void VisitPostOrder(T obj)
        {
            this.visitorToUse.Visit(obj);
        }

        /// <summary>
        /// Visits the object in order.
        /// </summary>
        /// <param name="obj">The obj.</param>
        public virtual void VisitInOrder(T obj)
        {
            this.visitorToUse.Visit(obj);
        }

        /// <inheritdoc />
        public void Visit(T obj)
        {
            this.visitorToUse.Visit(obj);
        }
    }
}
