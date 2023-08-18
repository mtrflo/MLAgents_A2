from mlagents_envs.environment import UnityEnvironment

# This is a non-blocking call that only loads the environment.
env = UnityEnvironment(file_name=None, seed=1, side_channels=[])

# Start interacting with the environment.
env.reset()

behavior_names = env.behavior_specs.keys()