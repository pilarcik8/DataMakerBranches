#!/bin/bash

for i in {0..4}; do
    diff3 -m createdFiles/left$i.xml createdFiles/base$i.xml createdFiles/right$i.xml > output/out$i.xml
done

for i in {0..4}; do
    echo "===== out$i.xml ====="
    cat "output/out$i.xml"
    echo ""
done
