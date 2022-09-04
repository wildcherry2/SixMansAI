import tensorflow as tf
import pandas as panda
import numpy as num
import keras_tuner as kt

from tensorflow.keras.layers import Dense, Normalization
from tensorflow.keras import Sequential
from sklearn.model_selection import train_test_split

def model_builder(hp):
  model = Sequential()
  #model.add(keras.layers.Flatten(input_shape=(28, 28)))

  # Tune the number of units in the first Dense layer
  # Choose an optimal value between 32-512
  hp_units = hp.Int('units', min_value=32, max_value=1024, step=16)
  factor = 0;
  model.add(Dense(units=hp_units, activation='swish',kernel_initializer='he_normal'))
  model.add(Dense(hp_units - (factor * (hp_units/8)), activation="swish", kernel_initializer='variance_scaling'))
  factor += 1
  model.add(Dense(hp_units - (factor * (hp_units/8)), activation="swish", kernel_initializer='variance_scaling'))
  factor += 1
  model.add(Dense(hp_units - (factor * (hp_units/8)), activation="swish", kernel_initializer='variance_scaling'))
  factor += 1
  model.add(Dense(hp_units - (factor * (hp_units/8)), activation="swish", kernel_initializer='variance_scaling'))
  factor += 1
  model.add(Dense(hp_units - (factor * (hp_units/8)), activation="swish", kernel_initializer='variance_scaling'))
  factor += 1
  model.add(Dense(hp_units - (factor * (hp_units/8)), activation="swish", kernel_initializer='variance_scaling'))
  factor += 1
  model.add(Dense(hp_units - (factor * (hp_units/8)), activation="swish", kernel_initializer='glorot_normal'))
  model.add(Dense(1, activation="sigmoid"))

  # Tune the learning rate for the optimizer
  # Choose an optimal value from 0.01, 0.001, or 0.0001
  hp_learning_rate = hp.Choice('learning_rate', values=[1e-2, 1e-3, 1e-4])

  model.compile(optimizer=tf.keras.optimizers.Adam(learning_rate=hp_learning_rate),
                loss=tf.keras.losses.Huber(),
                metrics=['accuracy'])

  return model

all_data = panda.read_csv("C:\\Users\\tyler\Documents\\Programming\\AI\\SixMans\\Database\\AIModel\\data_9_3_2022 11-34-57 AM.csv",
                          names=["T1P1 Season Wins","T1P1 Season Losses","T1P1 Total Wins","T1P1 Total Losses",
                                 "T1P2 Season Wins","T1P2 Season Losses","T1P2 Total Wins","T1P2 Total Losses",
                                 "T1P3 Season Wins","T1P3 Season Losses","T1P3 Total Wins","T1P3 Total Losses",
                                 "T2P1 Season Wins","T2P1 Season Losses","T2P1 Total Wins","T2P1 Total Losses",
                                 "T2P2 Season Wins","T2P2 Season Losses","T2P2 Total Wins","T2P2 Total Losses",
                                 "T2P3 Season Wins","T2P3 Season Losses","T2P3 Total Wins","T2P3 Total Losses",
                                 "Winner"])
features = all_data.copy();
labels = features.pop("Winner")

features_train, features_test, labels_train, labels_test = train_test_split(features, labels, test_size=0.2)

tuner = kt.Hyperband(model_builder,
                     objective='val_accuracy',
                     max_epochs=144,
                     factor=3,
                     hyperband_iterations=10,
                     overwrite=True
                     )
stop_early = tf.keras.callbacks.EarlyStopping(monitor='val_loss', patience=5)
tuner.search(features_train, labels_train, epochs=144, validation_split=0.2, callbacks=[stop_early])
best_hps=tuner.get_best_hyperparameters(num_trials=1)[0]

print(f"""
The hyperparameter search is complete. The optimal number of units in the first densely-connected
layer is {best_hps.get('units')} and the optimal learning rate for the optimizer
is {best_hps.get('learning_rate')}.
""")

model = tuner.hypermodel.build(best_hps)
history = model.fit(features_train, labels_train, epochs=144, validation_split=0.2)

val_acc_per_epoch = history.history['val_accuracy']
best_epoch = val_acc_per_epoch.index(max(val_acc_per_epoch)) + 1
print('Best epoch: %d' % (best_epoch,))

hypermodel = tuner.hypermodel.build(best_hps)

# Retrain the model
hypermodel.fit(features_train, labels_train, epochs=best_epoch, validation_split=0.2)
eval_result = hypermodel.evaluate(features_test, labels_test)
print("[test loss, test accuracy]:", eval_result)

#model = Sequential()

#model.add(Dense(42, activation="swish", kernel_initializer='glorot_normal', input_shape=(features_train.shape[1],)))
#model.add(Dense(35, activation="swish", kernel_initializer='variance_scaling'))
#model.add(Dense(28, activation="swish", kernel_initializer='variance_scaling'))
#model.add(Dense(21, activation="swish", kernel_initializer='variance_scaling'))
#model.add(Dense(14, activation="swish", kernel_initializer='variance_scaling'))
#model.add(Dense(7, activation="swish", kernel_initializer='glorot_normal'))
#model.add(Dense(1, activation="sigmoid"))
#model.compile(optimizer=tf.keras.optimizers.Adam(), loss=tf.keras.losses.BinaryCrossentropy(), metrics=["accuracy"])
#model.fit(features_train, labels_train, epochs=256, batch_size=32)

#loss, acc = model.evaluate(features_test, labels_test)
#print('Test Accuracy: %.3f' % acc)
#print('Test Loss: %.3f' % loss)
#print(all_data.head())
#mnist = tf.keras.datasets.mnist

#(x_train, y_train),(x_test, y_test) = mnist.load_data()
#x_train, x_test = x_train / 255.0, x_test / 255.0

#model = tf.keras.models.Sequential([
#  tf.keras.layers.Flatten(input_shape=(28, 28)),
#  tf.keras.layers.Dense(128, activation='relu'),
#  tf.keras.layers.Dropout(0.2),
#  tf.keras.layers.Dense(10, activation='softmax')
#])

#model.compile(optimizer='adam',
#              loss='sparse_categorical_crossentropy',
#              metrics=['accuracy'])

#model.fit(x_train, y_train, epochs=5)
#model.evaluate(x_test, y_test)