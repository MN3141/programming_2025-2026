import json
from perlin_noise import PerlinNoise
import numpy as np

class Particle:
    def __init__(self):
        self.best_found_config = 999
        self.position = None
        self.velocity = None

particles = []
configs = {}
noise = PerlinNoise(octaves=10, seed=5)
xpix, ypix = 500, 500

def _init():

    global configs
    file_handle = open('config.json', 'r')
    configs = json.load(file_handle)
    file_handle.close()

    global particles
    num_particles = configs["particle_number"]

    for _ in range(num_particles):
        particle = Particle()
        particle.position = np.array([int(np.random.uniform(0,xpix)), int(np.random.uniform(0,ypix))])
        particle.velocity = np.array([0.0, 0.0])
        particles.append(particle)

    print(f"Initialized {num_particles} particles.")

def _get_noise(x,y): # our fit function
  return noise([x, y])

def main():
    _init()

    particle_number = configs["particle_number"]
    best_global_config = 99999
    search_iterations = configs["iteration_limit"]

    while search_iterations > 0:

        last_best_global_config = best_global_config

        for i in range(particle_number):

            # update velocity and position
            w = configs["velocity_weight"]
            particles[i].velocity = w * particles[i].velocity + np.array(best_global_config - particles[i].position)
            particles[i].position = particles[i].position + particles[i].velocity

            # compute fitness
            fitness = _get_noise(particles[i].position[0]/xpix, particles[i].position[1]/ypix)
            if fitness < particles[i].best_found_config:
                particles[i].best_found_config = fitness

                if fitness < best_global_config:
                    best_global_config = fitness

        if last_best_global_config == best_global_config:
            search_iterations -= 1
        else:
            search_iterations = configs["iteration_limit"]

    print(f"Best global config: {best_global_config}")

main()