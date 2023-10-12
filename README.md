# Video Audio Transfer

The **Video Audio Transfer** application is a simple tool that allow transfer audio between video files. Built with .NET 7, this application leverages the capabilities of FFmpeg to process video files, so it's essential to have FFmpeg installed on your system.

## Features

Currently, the application offers the following functionality:

- **Video-to-Video Audio Transfer:** Transfer audio from one video to another.

- **Drag and Drop Support:** Allowing users to simply drag and drop files into the application. You can also clicking on the white box and choose a video file manually.

- The output will be export in the same folder as the exe, using the filename of the video that take audio, IT WILL OVERWRITE IF THERE IS A VIDEO WITH A SAME NAME so make sure to not put the inputs in the same folder as the exe.

## Future Plans

- **Audio-to-Video Transfer:** Expand the application's capabilities to support transferring audio into video files.

## Dependencies

- **FFmpeg:** As the application relies on FFmpeg for video processing, you should have FFmpeg installed on your system.

## Getting Started

If you want to work on the codebase:

1. **Download and Install .NET 7:**
   If you haven't already, you can download and install .NET 7 from the official .NET website.

2. **Install FFmpeg:**
   Visit the FFmpeg website and follow their installation instructions to set up FFmpeg on your system.

3. **Clone the Repository:**
   Clone the repository to your local machine using Git.

   ```bash
   git clone https://github.com/GrampHoang/VideoAudioTransfer.git

   <!-- dotnet publish -r win-x64 -p:PublishSingleFile=true --self-contained true -c Release   -->