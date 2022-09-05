import tensorflow as tf
import pandas as panda
import numpy as num

from tensorflow.keras.layers import Dense
from tensorflow.keras import Sequential
from sklearn.model_selection import train_test_split
from matplotlib import pyplot
from sklearn.preprocessing import MinMaxScaler, StandardScaler

all_data = panda.read_csv(r"C:\Users\tyler\Documents\Programming\AI\SixMans\Reports\data_9_4_2022 2-04-07 PM.csv",
                          names=["T1P1 Relative Rank","T1P2 Relative Rank","T1P3 Relative Rank","T1 Bias",
                                 "T2P1 Relative Rank","T2P2 Relative Rank","T2P3 Relative Rank","T2 Bias",
                                 "Winner"])

features = all_data.copy();
labels = features.pop("Winner")

features_train, features_test, labels_train, labels_test = train_test_split(features, labels, test_size=0.33)
#features_train = features_train.astype('float64')
normalize = tf.keras.layers.Normalization()
normalize.adapt(features_train)
print(normalize)
#features_train = features_train / 1000000000
print(features_train.sample(5))
model = Sequential()
model.add(normalize)
model.add(Dense(8, activation='swish', input_shape=(8,)))
model.add(Dense(30, activation='swish'))
model.add(Dense(24, activation='swish'))
model.add(Dense(18, activation='swish'))
model.add(Dense(10, activation='swish'))
model.add(Dense(4, activation='swish'))
#last_one = tf.keras.layers.InputLayer(Dense(1, activation='sigmoid', dtype='float64'))
#ep = 1e-8;
model.add(Dense(1, activation='sigmoid'))
model.compile(optimizer=tf.keras.optimizers.Adam(clipvalue=5., learning_rate=0.001), loss=tf.keras.losses.Huber(), metrics=["accuracy"])
hist = model.fit(features_train, labels_train, epochs=128, batch_size=32)

loss, acc = model.evaluate(features_test, labels_test)
pyplot.title('Learning Curves')
pyplot.xlabel('Epoch')
pyplot.ylabel('Cross Entropy')
pyplot.plot(hist.history['loss'], label='train')
pyplot.plot(hist.history['val_loss'], label='val')
pyplot.legend()
print('Test Accuracy: %.3f' % acc)
print('Test Loss: %.3f' % loss)
pyplot.show()
