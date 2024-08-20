//*****************************************************************************
//** 1140 Stone Game II   leetcode                                           **
//** Find the stones with a recursive function and store the optimal solution**
//** in a hash table.                                                -Dan    **
//*****************************************************************************


int* sum;
int** hash;
int N;

int finder(int* piles, int start, int M) {
    if (start == N) {
        return 0;
    }
    // The remaining size <= max stones we can take
    if (N - start <= 2 * M) {
        return sum[start];
    }
    // This cell has already been filled
    if (hash[start][M] != 0) {
        return hash[start][M];
    }

    int opponent = INT_MAX; // The minimum stone your opponent can take
    for (int i = 1; i <= 2 * M; i++) {
        if (start + i < N) {
            int opponent_value = finder(piles, start + i, M > i ? M : i);
            if (opponent_value < opponent) {
                opponent = opponent_value;
            }
        }
    }

    hash[start][M] = sum[start] - opponent;
    return hash[start][M];
}

int stoneGameII(int* piles, int pilesSize) {
    N = pilesSize;
    if (N == 0) {
        return 0;
    }

    // Allocate memory for sum array
    sum = (int*)malloc(N * sizeof(int));
    if (sum == NULL) {
        return -1; // Error in allocation
    }

    // Fill sum array
    for (int i = N - 1; i >= 0; i--) {
        sum[i] = piles[i];
        if (i + 1 < N) {
            sum[i] += sum[i + 1];
        }
    }

    // Allocate memory for hash table
    hash = (int**)malloc(N * sizeof(int*));
    if (hash == NULL) {
        free(sum); // Free sum if hash allocation fails
        return -1;
    }

    for (int i = 0; i < N; i++) {
        hash[i] = (int*)calloc(N, sizeof(int)); // Initialize hash with zeros
        if (hash[i] == NULL) {
            for (int j = 0; j < i; j++) {
                free(hash[j]); // Free previous allocations on failure
            }
            free(hash);
            free(sum);
            return -1;
        }
    }

    int result = finder(piles, 0, 1);

    // Free allocated memory
    for (int i = 0; i < N; i++) {
        free(hash[i]);
    }
    free(hash);
    free(sum);

    return result;
}