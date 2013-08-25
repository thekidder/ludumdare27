#!/bin/bash
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

rm $DIR/../unity/Assets/Levels/one.txt*
cp $DIR/one.json $DIR/../unity/Assets/Levels/one.txt