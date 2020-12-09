namespace Data.Bundles
{
    public interface IBundlable
    {
        public void StoreInBundle(Bundle bundle);
        public void RestoreFromBundle(Bundle bundle);
    }
}