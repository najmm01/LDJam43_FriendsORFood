public class ParticlesComponent : PooledObject
{
    //monobehavior callback when particle effect finishes playing
    void OnParticleSystemStopped()
    {
        //The following call returns the particle object to its pool if it exists, otherwise it gets destroyed
        ReturnToPool();
    }
}
