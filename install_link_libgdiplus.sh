#!/usr/bin/env bash
# This script is required for System.Drawing.Common
# For more information check out this answer of an github issue
# https://github.com/JanKallman/EPPlus/issues/83#issuecomment-404570402
sudo ln -s /lib/x86_64-linux-gnu/libdl.so.2 /lib/x86_64-linux-gnu/libdl.so
sudo apt update
sudo apt install -y libgdiplus
sudo ln -s /usr/lib/libgdiplus.so /lib/x86_64-linux-gnu/libgdiplus.so
