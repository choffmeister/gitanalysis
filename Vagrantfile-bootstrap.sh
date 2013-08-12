#!/usr/bin/env bash

# update packages
sudo aptitude update

# install vim
sudo aptitude install vim

# install build dependencies for libgit2
sudo aptitude install --assume-yes git
sudo aptitude install --assume-yes build-essential
sudo aptitude install --assume-yes cmake
sudo aptitude install --assume-yes libssl-dev

# install mono
sudo aptitude install --assume-yes mono-devel mono-gmcs nunit-console
mozroots --import --sync

# build libgit2 (9d9fff3)
git clone git://github.com/libgit2/libgit2.git
cd libgit2
git checkout 9d9fff3
mkdir build
cd build
cmake ..
cmake --build

# install libgit2 (9d9fff3)
sudo make install
sudo ln -s /usr/local/lib/libgit2.so.0.18.0 /usr/local/lib/libgit2-9d9fff3.so
