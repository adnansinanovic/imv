namespace Sinantrop.IMV.Db
{
  public abstract class Entity<TId>
  {
        public virtual TId Id { get; set; }
  }
}
