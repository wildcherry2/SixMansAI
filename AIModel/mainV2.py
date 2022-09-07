from cmath import exp
from pickle import TRUE
from wsgiref import validate
import tensorflow as tf
import pandas as panda
import numpy as num
import os
import datetime

from tensorflow.keras.layers import Dense
from tensorflow.keras import Sequential
from sklearn.model_selection import train_test_split
from matplotlib import pyplot
from sklearn.preprocessing import MinMaxScaler, StandardScaler

all_data = panda.read_csv(r"C:\Users\tyler\Documents\Programming\AI\SixMans\Reports\data_9_5_2022 11-14-53 PM.csv",
                          names=["T1P1 Season Differential","T1P1 Season Total","T1P1 Overall Differential", "T1P1 Total Games",
                                 "T1P2 Season Differential","T1P2 Season Total","T1P2 Overall Differential", "T1P2 Total Games",
                                 "T1P3 Season Differential","T1P3 Season Total","T1P3 Overall Differential", "T1P3 Total Games",
                                 "T2P1 Season Differential","T2P1 Season Total","T2P1 Overall Differential", "T2P1 Total Games",
                                 "T2P2 Season Differential","T2P2 Season Total","T2P2 Overall Differential", "T2P2 Total Games",
                                 "T2P3 Season Differential","T2P3 Season Total","T2P3 Overall Differential", "T2P3 Total Games",
                                 "Day", "Winner"])

features = all_data.copy();
labels = features.pop("Winner")

fig = pyplot.figure(1,(18,10))
pyplot.title('Learning Curves')
pyplot.xlabel('Epoch')
pyplot.ylabel('Loss')
graph = {}

def DoTrain(num_nodes, train_color, num_epochs = 128, num_layers = 8, initializer = "he_uniform", opt = tf.keras.optimizers.Adam(), exp_label = "Adam (default)"):
    features_train, features_test, labels_train, labels_test = train_test_split(features, labels, test_size=0.3)
    model = Sequential()
    ret = ""
    for x in range(0, num_layers):
        model.add(Dense(num_nodes, activation='swish', kernel_initializer=initializer))

    model.add(Dense(1, activation='sigmoid'))
    model.compile(optimizer=opt, loss=tf.keras.losses.Huber(), metrics=["accuracy"])
    hist = model.fit(features_train, labels_train, epochs=num_epochs, batch_size=32, validation_split=0.3)
    loss, acc = model.evaluate(features_test, labels_test)
    training_label = 'N = ' + str(num_nodes) + ', L = ' + str(num_layers) + ', Training loss' + ', O = ' + exp_label
    valid_label = 'N = ' + str(num_nodes) + ', L = ' + str(num_layers) + ', Validation loss' + ', O = ' + exp_label
    graph[training_label] = pyplot.plot(hist.history['loss'],train_color, label=training_label)
    graph[valid_label] = pyplot.plot(hist.history['val_loss'],train_color, label=valid_label)
    ret += ('(' + str(num_nodes) + ',' + str(num_layers) + ') TL = ' + str(loss) + "\n")
    ret +=('(' + str(num_nodes) + ',' + str(num_layers) + ') TA = ' + str(acc) + "\n")
    return ret

def OnPick(event):
    global fig
    legend = event.artist
    isVisible = legend.get_visible()

    legend.set_visible(not isVisible)
    graph[legend.get_label()][0].set_visible(not isVisible)

    fig.canvas.draw()

results = ""

results += DoTrain(25,'#fff370', 100, 30, "he_uniform", tf.keras.optimizers.Adam())
results += DoTrain(25,'#0000FF', 100, 30, "he_uniform", tf.keras.optimizers.Adam(amsgrad=True), "Adam(amsgrad = true)") #decent
results += DoTrain(25,'#02c775', 100, 30, "he_uniform", tf.keras.optimizers.Adam(learning_rate=0.01), "Adam(learning_rate = 0.01)")
results += DoTrain(25,'#8902c7', 100, 30, "he_uniform", tf.keras.optimizers.Adam(learning_rate=0.0001), "Adam(learning_rate = 0.0001)")
results += DoTrain(25,'#fc0303', 100, 30, "he_uniform", tf.keras.optimizers.Adam(learning_rate=0.01, amsgrad=True), "Adam(amsgrad = true, learning_rate = 0.01)") #insane
results += DoTrain(25,'#4287f5', 100, 30, "he_uniform", tf.keras.optimizers.Adam(learning_rate=0.0001, amsgrad=True), "Adam(amsgrad = true, learning_rate = 0.0001)")
results += "\n"
results += DoTrain(25,'#90eef5', 100, 30, "he_uniform", tf.keras.optimizers.SGD(), "SGD(default)")
results += DoTrain(25,'#eb4034', 100, 30, "he_uniform", tf.keras.optimizers.SGD(nesterov=True), "SGD(nesterov = true)") #decent
results += DoTrain(25,'#32a852', 100, 30, "he_uniform", tf.keras.optimizers.SGD(learning_rate=0.001), "SGD(learning_rate = 0.001)")
results += DoTrain(25,'#f740b1', 100, 30, "he_uniform", tf.keras.optimizers.SGD(learning_rate=0.0001), "SGD(learning_rate = 0.0001)")
results += DoTrain(25,'#116d58', 100, 30, "he_uniform", tf.keras.optimizers.SGD(learning_rate=0.001, nesterov=True), "SGD(nesterov = true, learning_rate = 0.001)")
results += DoTrain(25,'#576764', 100, 30, "he_uniform", tf.keras.optimizers.SGD(learning_rate=0.0001, nesterov=True), "SGD(nesterov = true, learning_rate = 0.0001)")

print(results)
legend = pyplot.legend()
lines = legend.get_lines()

for line in lines:
    line.set_picker(True)
    line.set_pickradius(7)


outdir = "Results\\" + datetime.datetime.now;
#pyplot.savefig("")
pyplot.connect('pick_event', OnPick)
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