import tensorflow as tf
import pandas as panda
import numpy as num

from tensorflow.keras.layers import Dense
from tensorflow.keras import Sequential
from sklearn.model_selection import train_test_split
from matplotlib import pyplot
from sklearn.preprocessing import MinMaxScaler, StandardScaler

all_data = panda.read_csv(r"C:\Users\tyler\Documents\Programming\AI\SixMans\Reports\data_9_5_2022 2-17-06 AM.csv",
                          names=["T1P1 Relative Rank","T1P2 Relative Rank","T1P3 Relative Rank",
                                 "T2P1 Relative Rank","T2P2 Relative Rank","T2P3 Relative Rank",
                                 "Winner"])

features = all_data.copy();
labels = features.pop("Winner")

pyplot.figure(1,(18,10))
pyplot.title('Learning Curves')
pyplot.xlabel('Epoch')
pyplot.ylabel('Loss')


def DoTrain(num_nodes, train_color, num_epochs = 128, num_layers = 8, initializer = "he_uniform"):
    features_train, features_test, labels_train, labels_test = train_test_split(features, labels, test_size=0.33)
    model = Sequential()
    ret = ""
    for x in range(0, num_layers):
        model.add(Dense(num_nodes, activation='swish', kernel_initializer=initializer))

    model.add(Dense(1, activation='sigmoid'))
    model.compile(optimizer=tf.keras.optimizers.Adam(), loss=tf.keras.losses.Huber(), metrics=["accuracy"])
    hist = model.fit(features_train, labels_train, epochs=num_epochs, batch_size=32, shuffle=True, validation_split=0.3)
    loss, acc = model.evaluate(features_test, labels_test)
    pyplot.plot(hist.history['loss'],train_color, label='N = ' + str(num_nodes) + ', L = ' + str(num_layers) + ' Training loss')
    pyplot.plot(hist.history['val_loss'],train_color, label='N = ' + str(num_nodes) + ', L = ' + str(num_layers) + ' Validation loss')
    ret += ('(' + str(num_nodes) + ',' + str(num_layers) + ') TL = ' + str(loss) + "\n")
    ret +=('(' + str(num_nodes) + ',' + str(num_layers) + ') TA = ' + str(acc) + "\n")
    return ret

results = ""

results += DoTrain(6,'#02c775', 256, 6, "he_normal")
results += DoTrain(6,'#0000FF', 256, 5, "he_normal")
results += DoTrain(6,'#42f548', 256, 4, "he_normal")
results += DoTrain(6,'#fff370', 256, 3, "he_normal")
results += DoTrain(6,'#fc0303', 256, 2, "he_normal")
results += DoTrain(8,'#8902c7', 256, 8, "he_normal")

print(results)
pyplot.legend()
pyplot.show()

'''
BINARY CROSSENTROPY

10 x 8
Test Accuracy: 0.507
Test Loss: 0.918

12 x 8
Test Accuracy: 0.490
Test Loss: 1.382

12 x 7
Test Accuracy: 0.515
Test Loss: 1.140

14 x 8
Test Accuracy: 0.517
Test Loss: 1.961
'''

'''
HUBER
8 x 8 (swish)
Test Accuracy: 0.507
Test Loss: 0.147

8 x 8 (relu)
Test Accuracy: 0.510
Test Loss: 0.142
'''