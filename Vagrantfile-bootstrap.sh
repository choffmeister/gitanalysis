#!/usr/bin/env bash

sudo aptitude update
sudo aptitude install --assume-yes mono-devel mono-gmcs nunit-console
sudo aptitude install --assume-yes git
sudo aptitude install --assume-yes build-essential cmake
sudo aptitude install --assume-yes build-essential libssl-dev
