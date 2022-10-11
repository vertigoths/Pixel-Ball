using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voodoo.Visual
{
    public class Control : MonoBehaviour
    {
	    public ParticleSystem explosion;
	    public ParticleSystem explosionUp;
	    public ParticleSystem fountain;
	    public ParticleSystem screenExplosion;

	    public void UseExplosion()
	    {
		    explosion.Play();
	    }
	    
	    public void UseExplosionUp()
	    {
		    explosionUp.Play();
	    }
	    
	    public void UseFountain()
	    {
		    if (fountain.isEmitting)
		    {
			    fountain.Stop();
		    }
		    else
		    {
			    fountain.Play(); 
		    }
		    
	    }
	    
	    public void UseScreenExplosion()
	    {
		    screenExplosion.Play();
	    }

    }
}